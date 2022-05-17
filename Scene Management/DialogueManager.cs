using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private PlayerMover player;
    private PlayerScript playerScript;

    XmlDocument dialogueXml;
    XmlNode currentNode;

    // this list keeps track of the path taken to get to currentNode
    // ex: if the current node is
    private List<int> xmlTreeTracker;
    private int currentDepth;
    [System.NonSerialized]
    public bool isTalking = false;
    [System.NonSerialized]
    public DataManager gameData;
    private bool isYN = false;
    private float YNTimer = 0.0f;
    private bool YN;
    private bool silence;
    private bool[] YNCache = new bool[5];
    private bool[] silenceCache = new bool[5];



    public bool isSilence = false;
    private float silenceTimer = 0.0f;
    private float silenceEnd = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerScript>().characterMover;
        playerScript = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerScript>();
        gameData = FindObjectOfType<DataManager>().GetComponent<DataManager>();
        xmlTreeTracker = new List<int>();
    }

    void Update(){

        // update the timer keeping track of silence
        if(isSilence){
            silenceTimer += Time.deltaTime;
            if(silenceTimer > silenceEnd){
                isSilence = false;
            }
        }


        // update the timer keeping track of the lack of response to a Y/N question
        if(YNTimer > 0.0f){
            YNTimer -= Time.deltaTime;
            // answer window has passed and the player has not answered and chosen silence
            if(YNTimer <= 0.0f){
                if(currentNode.Attributes["cache"] == null){
                    silence = true;
                }
                else{
                    silence = true;
                    silenceCache[int.Parse(currentNode.Attributes["cache"].Value)] = true;
                }
                isYN = false;
                player.isYN = false;
                player.last3 = new int[] {0,0,0};
                currentNode = currentNode.NextSibling;
                xmlTreeTracker[currentDepth]++;
                HandleNextElement();
            }

            // player has answered the Y/N
            if(!player.isYN){
                isYN = false;
                YNTimer = 0.0f;
                if(currentNode.Attributes["cache"] == null){
                    YN = player.YNAnswer;
                    silence = false;
                }
                else{
                    YN = player.YNAnswer;
                    silence = false;
                    YNCache[int.Parse(currentNode.Attributes["cache"].Value)] = player.YNAnswer;
                    silenceCache[int.Parse(currentNode.Attributes["cache"].Value)] = false;
                }

                player.last3 = new int[] {0,0,0};
                currentNode = currentNode.NextSibling;
                xmlTreeTracker[currentDepth]++;
                HandleNextElement();
            }
        }
    }

    public bool StartDialogue(Dialogue dialogue, PlayerMover player)
    {
        player.EnterConversation();

        dialogueXml = new XmlDocument();
        dialogueXml.Load("Assets/Resources/Dialogue/" + dialogue.xmlFile + ".xml");

        xmlTreeTracker.Add(0);
        currentDepth = 0;
        currentNode = null;

        //isTalking = true;

        HandleNextElement();
        return true;
    }

    // causes of increase in depth:
    // branch based on story data (StoryFork)
    // branch based on previous y/n in conversation (YNCacheFork)
    // branch based on current y/n (YNFork)
    // branch based on what the player has in inventory (inventoryFork)
    // branch based on how many gems the player has (gemFork)
    // branch based on the health of the player (healthFork)
    // branch based on the health of the character (characterFork)

    // branch based on relationship to another character?
    //
    public bool HandleNextElement(){
        //Debug.Log("CHECKING ELEMENT" + xmlTreeTracker[currentDepth] + " AT DEPTH " + currentDepth);

        // set currentNode to the first meaningful node in the tree since dialogue is being initiated
        if(currentNode == null && !isTalking){
            currentNode = dialogueXml.FirstChild.NextSibling.FirstChild;
            isTalking = true;
        }

        // end of tree has been found at current depth
        if(currentNode == null && isTalking){
            // retract depth by one
            xmlTreeTracker.RemoveAt(currentDepth);
            currentDepth--;

            // check if there are no available conversation elements and exit the conversation if so
            if(currentDepth < 0){
                player.ExitConversation();
                animator.SetBool("IsOpen",false);
                isTalking = false;

                // reset tracker for next conversation
                xmlTreeTracker = new List<int>();
                xmlTreeTracker.Add(0);
                return false;
            }

            // recalibrate position of currentNode within the tree so that it is the next sibling of the parent of the
            // branch just completed
            currentNode = dialogueXml.FirstChild.NextSibling.FirstChild;
            for(int depth = 0; depth <= currentDepth; depth++){
                for(int element = 0; element < xmlTreeTracker[depth]; element++){
                    currentNode = currentNode.NextSibling;
                }
                return HandleNextElement();
            }

        }

        // DISPLAY DIALOGUE TEXT
        if(currentNode.Attributes["type"].Value.Equals("text")){
            dialogueText.text = currentNode["text"].InnerText;
            nameText.text = currentNode["name"].InnerText;
            animator.SetBool("IsOpen",true);
            currentNode = currentNode.NextSibling;
            xmlTreeTracker[currentDepth]++;
        }
        // SAVED DATA CHANGE
        else if(currentNode.Attributes["type"].Value.Equals("dataChange")){
            gameData.current.gameData[currentNode.Attributes["key"].Value] = bool.Parse(currentNode.Attributes["truth"].Value);
            currentNode = currentNode.NextSibling;
            xmlTreeTracker[currentDepth]++;
            return HandleNextElement();
        }
        // END CONVERSATION
        else if(currentNode.Attributes["type"].Value.Equals("end")){
                player.ExitConversation();
                animator.SetBool("IsOpen",false);
                isTalking = false;
                xmlTreeTracker = new List<int>();
                xmlTreeTracker.Add(0);
                return false;
        }
        // GIVE ITEM TO PLAYER
        // TAKE ITEM FROM PLAYER
        // YN
        else if(currentNode.Attributes["type"].Value.Equals("YN")){
            dialogueText.text = currentNode["text"].InnerText;
            nameText.text = currentNode["name"].InnerText;
            animator.SetBool("IsOpen",true);
            player.isYN = true;
            isYN = true;
            YNTimer = float.Parse(currentNode.Attributes["answerTime"].Value);
        }
        // BRANCHES
        else if(currentNode.Attributes["type"].Value.Equals("branch")){
            bool validBranch = true;
            int numRequirements = 0;

            // check all 'requirement' children of the branch to ensure the branch is valid
            foreach(XmlNode childNode in currentNode.ChildNodes){
                if(childNode.Name.Equals("requirement")){
                    // SAVED DATA REQUIREMENT
                    if(childNode.Attributes["type"].Value.Equals("savedReq")){
                        bool reqData = gameData.GetData(childNode.Attributes["key"].Value);
                        if(reqData != bool.Parse(childNode.Attributes["truth"].Value)){
                            validBranch = false;
                            break;
                        }
                    }
                    // INVENTORY CHECK
                    else if(childNode.Attributes["type"].Value.Equals("inventoryReq")){
                        bool foundItem = false;
                        for(int i = 0; i < playerScript.inventory.numItems; i++){
                            if(playerScript.inventory.items[i].id.Equals(childNode.Attributes["id"].Value)){
                                foundItem = true;
                                break;
                            }
                        }
                        if(!foundItem){
                            validBranch = false;
                        }
                    }
                    // (PREVIOUS) YN CHECK
                    else if(childNode.Attributes["type"].Value.Equals("YNReq")){
                        if(YN != bool.Parse(childNode.Attributes["truth"].Value)){
                            validBranch = false;
                            break;
                        }
                    }
                    numRequirements++;
                }
            }

            //
            if(validBranch){
                currentNode = currentNode.FirstChild;
                xmlTreeTracker[currentDepth]++;
                currentDepth++;

                // since currentNode is now one level deeper in the tree, update xmlTreeTracker to the correct
                // value for this depth and update currentNode to be the correcet child node
                xmlTreeTracker.Add(0);
                for(int i = 0; i < numRequirements; i ++){
                    currentNode = currentNode.NextSibling;
                    xmlTreeTracker[currentDepth]++;
                }
                return HandleNextElement();
            }
            else{
                currentNode = currentNode.NextSibling;
                xmlTreeTracker[currentDepth]++;
                return HandleNextElement();
            }
        }
        return true;
    }
}

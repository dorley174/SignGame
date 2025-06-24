using UnityEngine;

public class TorgashInteraction : MonoBehaviour
{
    [SerializeField] private GameObject letterF;
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private GameObject shopCanvas;

    private bool isPlayerNearby = false;
    private GameObject player;
    private bool active = false;
    
    //
    public Animator torgashAnimator;
    public bool isPanelActive = false;
    //

    private void Start()
    {
        letterF.SetActive(false);
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);
        isPlayerNearby = distance <= detectionRadius;
        letterF.SetActive(isPlayerNearby);

        //
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !active)
        {
            torgashAnimator.SetBool("open", true);

            // Открываем панель магазина
            shopCanvas.GetComponent<GUIManager>().PanelActivate(true);
            // isPanelActive = true;
        }
        else if (isPlayerNearby && Input.GetKeyDown(KeyCode.F) && active)
        {
            torgashAnimator.SetBool("open", false);
            shopCanvas.GetComponent<GUIManager>().PanelActivate(false);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            letterF.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            letterF.SetActive(false);
            
            torgashAnimator.SetBool("open", false);
        }
    }
}

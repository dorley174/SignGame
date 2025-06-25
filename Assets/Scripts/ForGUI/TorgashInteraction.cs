using System.Collections;
using UnityEngine;

public class TorgashInteraction : MonoBehaviour
{
    [SerializeField] private GameObject letterF;
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private GameObject shopCanvas;
    // [SerializeField] private GameObject cameraObject;

    private GameObject player;
    private PlayerController playerController;
    private bool isPlayerNearby = false;
    private bool isInteractive = false;

    private void Start()
    {
        letterF.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

    }

    private void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);
        isPlayerNearby = distance <= detectionRadius;
        letterF.SetActive(isPlayerNearby);

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !isInteractive)
        {
            StartCoroutine(HandleInteraction());
        }
    }

    private IEnumerator HandleInteraction()
    {
        isInteractive = true;

        GUIManager guiManager = shopCanvas.GetComponent<GUIManager>();

        if (!guiManager.IsPanelActive)
        {
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            // if (cameraObject != null)
            // {
            //     cameraObject.GetComponent<LineDrawer>().gameObject.SetActive(false);
            // }
            this.GetComponent<Animator>().SetBool("open", true);
            yield return new WaitForSeconds(0.3f);

            guiManager.PanelActivate(true);
        }
        else
        {
            guiManager.PanelActivate(false);
            yield return new WaitForSeconds(1.1f);

            this.GetComponent<Animator>().SetBool("open", false);

            if (playerController != null)
            {
                playerController.enabled = true;
            }
            // if (cameraObject != null)
            // {
            //     cameraObject.GetComponent<LineDrawer>().gameObject.SetActive(true);
            // }
        }

        isInteractive = false;
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
            
            shopCanvas.GetComponent<GUIManager>().PanelActivate(false);
            this.GetComponent<Animator>().SetBool("open", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeGraphics : MonoBehaviour
{

    public static UpgradeGraphics Main;

    [SerializeField]
    private List<GameObject> balloons;

    [SerializeField]
    private List<GameObject> rockets;

    [SerializeField]
    private List<GameObject> crystals;

    [SerializeField]
    private List<GameObject> cauldrons;

    [SerializeField]
    private List<GameObject> catchers;

    [SerializeField]
    private List<GameObject> bubbles;

    [SerializeField]
    private List<GameObject> devices;

    void Awake() {
        Main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetBalloonLevel(0);
        SetRocketLevel(0);
        SetCrystalLevel(0);
        SetCauldronLevel(0);
        SetBubbleLevel(0);
        SetCatcherLevel(0);
        SetDeviceLevel(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBalloonLevel(int level) {
        activateGameObjects(balloons, level);
    }

    public void SetRocketLevel(int level) {
        activateGameObjects(rockets, level);
    }

    public void SetCrystalLevel(int level) {
        activateGameObjects(crystals, level);
    }

    public void SetCauldronLevel(int level) {
        activateGameObjects(cauldrons, level);
    }

    public void SetCatcherLevel(int level) {
        activateGameObjects(catchers, level);
    }

    public void SetBubbleLevel(int level) {
        activateGameObjects(bubbles, level);
    }

    public void SetDeviceLevel(int level) {
        activateGameObjects(devices, level);
    }

    private void activateGameObjects(List<GameObject> objects, int count) {
        for (var i = 0; i < objects.Count; i++) {
            objects[i].SetActive(i < count);
        }
    }
}

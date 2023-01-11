using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject Generator;
    public Farmer farmer;
    public Store store;

    public Text collected;
    public Text storage;
    public Text cState;
    public Text pState;

    void Start()
    {
        Instantiate(Generator, new Vector3(Random.Range(20f, 60f), 2f, Random.Range(20f, 60f)), new Quaternion(0f, 0f, 0f, 0f));
        Instantiate(Generator, new Vector3(Random.Range(20f, 60f), 2f, Random.Range(-60f, -20f)), new Quaternion(0f, 0f, 0f, 0f));
        Instantiate(Generator, new Vector3(Random.Range(-60f, -20f), 2f, Random.Range(-60f, -20f)), new Quaternion(0f, 0f, 0f, 0f));
        Instantiate(Generator, new Vector3(Random.Range(-60f, -20f), 2f, Random.Range(20f, 60f)), new Quaternion(0f, 0f, 0f, 0f));
    }

    void Update()
    {
        collected.text = "Collected: " + farmer.collected;
        storage.text = "Storage: " + store.currentStorage;
        cState.text = "Current State:\n" + farmer.currentStateText;
        pState.text = "Previous State:\n" + farmer.prevStateText;
    }
}

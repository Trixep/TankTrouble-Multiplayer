using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class skinSelector : MonoBehaviour
{
    public static skinSelector Instance { get; private set; }

    [SerializeField] private Button skinPreviewButton;
    [SerializeField] private Button skinChangerButton;
    [SerializeField] private Button doneButton;
    [SerializeField] private GameObject skinSelectorWindow;
    [SerializeField] private GameObject startWindow;
    [SerializeField] private GameObject lobbyWindow;

    private bool inLobby;

    [SerializeField] private Image[] towerRenderer;
    [SerializeField] private Image[] gunRenderer;
    [SerializeField] private Image[] gunConnectorRenderer;
    [SerializeField] private Image[] bodyRenderer;
    [SerializeField] private Image[] track1Renderer;
    [SerializeField] private Image[] track2Renderer;
    [SerializeField] private Image[] colorRenderer;

    private int towerId;
    private int bodyId;
    private int trackId;
    private int colorId;
    private string skinIds;

    [SerializeField] private Sprite[] towers;
    [SerializeField] private Sprite[] connectors;
    [SerializeField] private Sprite[] guns;
    [SerializeField] private Sprite[] bodies;
    [SerializeField] private Sprite[] tracks;

    [SerializeField] float[] towerSize = new float[9];
    [SerializeField] float[] bodySize = new float[9];

    [SerializeField] float[] towerPozX = new float[9];
    [SerializeField] float[] towerPozY = new float[9];

    [SerializeField] float[] connectorPozX = new float[9];
    [SerializeField] float[] connectorPozY = new float[9];

    [SerializeField] float[] gunPozX = new float[9];
    [SerializeField] float[] gunpozY = new float[9];

    [SerializeField] float[] trackPozX1 = new float[4];
    [SerializeField] float[] trackPozX2 = new float[4];
    [SerializeField] float[] trackPozY = new float[4];

    private Dictionary<string, string> colors = new Dictionary<string, string>()
    {
        {"Azure", "#007FFF"},
        {"Blue", "#0000FF"},
        {"Navy", "#000080"},
        {"Yellow", "#F5deb3"},
        {"Golden", "#d4af37"},
        {"Orange", "#FFA500"},
        {"Coral", "#f88379"},
        {"White", "#FFFFFF"},
        {"Silver", "#c0c0c0"},
        {"Gray", "#808080"},
        {"Charcoal", "#36454f"},
        {"Black", "#000000"},
        {"Brown", "#964B00"},
        {"Khaki", "#c3b091"},
        {"Red", "#FF0000"},
        {"Maroon", "#800000"},
        {"Fuchsia", "#ff77ff"},
        {"Pink", "#FF00FF"},
        {"Purple", "#8A2BE2"},
        {"Plum", "#673147"},
        {"Cyan", "#00FFFF"},
        {"Aquamarine", "#7fffd4"},
        {"Teal", "#008080"},
        {"Green", "#00FF00"},
        {"Lime", "#32CD32"},
    };

    private void Awake()
    {
        Instance = this;

        towerId = PlayerPrefs.GetInt("tower", 4);
        bodyId = PlayerPrefs.GetInt("body", 4);
        trackId = PlayerPrefs.GetInt("track", 0);
        colorId = PlayerPrefs.GetInt("color", 8);

        skinPreviewButton.onClick.AddListener(() =>
        {
            inLobby = false;
        });

        skinChangerButton.onClick.AddListener(() =>
        {
            inLobby = true;
        });

        doneButton.onClick.AddListener(() =>
        {
            if (inLobby == false)
            {
                skinSelectorWindow.SetActive(false);
                startWindow.SetActive(true);
            }
            else
            {
                skinSelectorWindow.SetActive(false);
                lobbyWindow.SetActive(true);
                LobbyOperator.Instance.UpdatePlayerCharacter(GetSkinId());
            }
        });
    }

    private void Start()
    {
        setItem("towers");
        setItem("bodies");
        setItem("tracks");
        setItem("colors");

    }

    public void selectTowers(bool isFoward)
    {
        if (isFoward)
        {
            if (towerId == towers.Length - 1)
            {
                towerId = 0;
            }
            else
            {
                towerId++;
            }
        }
        else
        {
            if (towerId == 0)
            {
                towerId = towers.Length - 1;
            }
            else
            {
                towerId--;
            }
        }

        PlayerPrefs.SetInt("tower", towerId);
        setItem("towers");
    }

    public void selectBodies(bool isFoward)
    {
        if (isFoward)
        {
            if (bodyId == bodies.Length - 1)
            {
                bodyId = 0;
            }
            else
            {
                bodyId++;
            }
        }
        else
        {
            if (bodyId == 0)
            {
                bodyId = bodies.Length - 1;
            }
            else
            {
                bodyId--;
            }
        }

        PlayerPrefs.SetInt("body", bodyId);
        setItem("bodies");
    }

    public void selectTracks(bool isFoward)
    {
        if (isFoward)
        {
            if (trackId == tracks.Length - 1)
            {
                trackId = 0;
            }
            else
            {
                trackId++;
            }
        }
        else
        {
            if (trackId == 0)
            {
                trackId = tracks.Length - 1;
            }
            else
            {
                trackId--;
            }
        }

        PlayerPrefs.SetInt("track", trackId);
        setItem("tracks");
    }

    public void selectColors(bool isFoward)
    {
        if (isFoward)
        {
            if (colorId == colors.Count - 1)
            {
                colorId = 0;
            }
            else
            {
                colorId++;
            }
        }
        else
        {
            if (colorId == 0)
            {
                colorId = colors.Count - 1;
            }
            else
            {
                colorId--;
            }
        }

        PlayerPrefs.SetInt("color", colorId);
        setItem("colors");
    }

    public void setItem(string type)
    {
        switch (type)
        {
            case "towers":
                for (int i = 0; i < towerRenderer.Length; i++)
                {
                    towerRenderer[i].sprite = towers[towerId];
                    gunRenderer[i].sprite = guns[towerId];
                    if (towerId == 0)
                    {
                        gunConnectorRenderer[i].sprite = connectors[0];
                    }
                    if (towerId == 1)
                    {
                        gunConnectorRenderer[i].sprite = connectors[1];
                    }
                    if (towerId == 2)
                    {
                        gunConnectorRenderer[i].sprite = connectors[2];
                    }
                    if (towerId >= 3 && towerId <= 5)
                    {
                        gunConnectorRenderer[i].sprite = connectors[3];
                    }
                    if (towerId >= 6 && towerId <= 8)
                    {
                        gunConnectorRenderer[i].sprite = connectors[4];
                    }

                    towerRenderer[i].transform.localScale = new Vector3(towerSize[towerId], towerSize[towerId], 1);
                    towerRenderer[i].transform.localPosition = new Vector3(towerPozX[towerId], towerPozY[towerId], 1);
                    gunConnectorRenderer[i].transform.localPosition = new Vector3(connectorPozX[towerId], connectorPozY[towerId], 1);
                    gunRenderer[i].transform.localPosition = new Vector3(gunPozX[towerId], gunpozY[towerId], 1);
                }
                break;

            case "bodies":
                for (int i = 0; i < bodyRenderer.Length; i++)
                {
                    bodyRenderer[i].sprite = bodies[bodyId];
                    bodyRenderer[i].transform.localScale = new Vector3(bodySize[bodyId], bodySize[bodyId], 1);
                }
                break;

            case "tracks":
                for (int i = 0; i < bodyRenderer.Length; i++)
                {
                    track1Renderer[i].sprite = tracks[trackId];
                    track2Renderer[i].sprite = tracks[trackId];
                    track1Renderer[i].transform.localPosition = new Vector3(trackPozX1[trackId], trackPozY[trackId], 1);
                }
                break;

            case "colors":
                if(ColorUtility.TryParseHtmlString(colors.Values.ElementAt(colorId), out Color color))
                {
                    for (int i = 0; i < bodyRenderer.Length; i++)
                    {
                        colorRenderer[i].color = color;
                    }
                }
                break;
        }
    }

    public string GetSkinId() 
    {
        skinIds = towerId.ToString() + ";" + bodyId.ToString() + ";" + trackId.ToString() + ";" + colorId.ToString();
        return skinIds;
    }

    public Vector3 GetTransform(string type, int id)
    {
        switch (type)
        {
            case "towerScale":
                return new Vector3(towerSize[id], towerSize[id], 1);

            case "towerPos":
                return new Vector3(towerPozX[id], towerPozY[id], 1);

            case "gunPos":
                return new Vector3(gunPozX[id], gunpozY[id], 1);

            case "gunConnectorPos":
                return new Vector3(connectorPozX[id], connectorPozY[id], 1);

            case "bodyScale":
                return new Vector3(bodySize[id], bodySize[id], 1);

            case "track1Pos":
                return new Vector3(trackPozX1[id], trackPozY[id], 1);
        }

        return new Vector3(1, 1, 1);
    }

    public Sprite GetSkin(string type, int id)
    {
        switch (type)
        {
            case "tower":
                return towers[id];

            case "gun":
                return guns[id];

            case "gunConnector":
                if (id == 0)
                {
                    return connectors[0];
                }
                if (id == 1)
                {
                    return connectors[1];
                }
                if (id == 2)
                {
                    return connectors[2];
                }
                if (id >= 3 && towerId <= 5)
                {
                    return connectors[3];
                }
                else
                {
                    return connectors[4];
                }

            case "body":
                return bodies[id];

            case "track1":
                return tracks[id];

            case "track2":
                return tracks[id];
        }

        return null;
    }

    public Color GetColor(int id)
    {
        if (ColorUtility.TryParseHtmlString(colors.Values.ElementAt(id), out Color color))
        {
            return color;
        }

        return Color.black;
    }
}

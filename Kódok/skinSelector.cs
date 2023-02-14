using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class skinSelector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer towerRendererSelect;
    [SerializeField] private SpriteRenderer connectorRendererSelect;
    [SerializeField] private SpriteRenderer gunRendererSelect;
    [SerializeField] private SpriteRenderer bodyRendererSelect;
    [SerializeField] private SpriteRenderer trackRenderer1Select;
    [SerializeField] private SpriteRenderer trackRenderer2Select;
    [SerializeField] private SpriteRenderer colorRendererSelect;
    [SerializeField] private SpriteRenderer towerRendererPrev;
    [SerializeField] private SpriteRenderer connectorRendererPrev;
    [SerializeField] private SpriteRenderer gunRendererPrev;
    [SerializeField] private SpriteRenderer bodyRendererPrev;
    [SerializeField] private SpriteRenderer trackRenderer1Prev;
    [SerializeField] private SpriteRenderer trackRenderer2Prev;
    [SerializeField] private SpriteRenderer colorRendererPrev;

    private int towerId;
    private int bodyId;
    private int trackId;
    private int colorId;

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
        towerId = PlayerPrefs.GetInt("tower", 4);
        bodyId = PlayerPrefs.GetInt("body", 4);
        trackId = PlayerPrefs.GetInt("track", 0);
        colorId = PlayerPrefs.GetInt("color", 2);
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

    private void setItem(string type)
    {
        switch (type)
        {
            case "towers":
                towerRendererSelect.sprite = towers[towerId];
                gunRendererSelect.sprite = guns[towerId];
                towerRendererPrev.sprite = towers[towerId];
                gunRendererPrev.sprite = guns[towerId];
                if (towerId == 0)
                {
                    connectorRendererSelect.sprite = connectors[0];
                    connectorRendererPrev.sprite = connectors[0];
                }
                if (towerId == 1)
                {
                    connectorRendererSelect.sprite = connectors[1];
                    connectorRendererPrev.sprite = connectors[1];
                }
                if (towerId == 2)
                {
                    connectorRendererSelect.sprite = connectors[2];
                    connectorRendererPrev.sprite = connectors[2];
                }
                if (towerId >= 3 && towerId <= 5)
                {
                    connectorRendererSelect.sprite = connectors[3];
                    connectorRendererPrev.sprite = connectors[3];
                }
                if (towerId >= 6 && towerId <= 8)
                {
                    connectorRendererSelect.sprite = connectors[4];
                    connectorRendererPrev.sprite = connectors[4];
                }

                towerRendererSelect.transform.localScale = new Vector2(towerSize[towerId], towerSize[towerId]);
                towerRendererSelect.transform.localPosition = new Vector2(towerPozX[towerId], towerPozY[towerId]);
                connectorRendererSelect.transform.localPosition = new Vector2(connectorPozX[towerId], connectorPozY[towerId]);
                gunRendererSelect.transform.localPosition = new Vector2(gunPozX[towerId], gunpozY[towerId]);

                towerRendererPrev.transform.localScale = new Vector2(towerSize[towerId], towerSize[towerId]);
                towerRendererPrev.transform.localPosition = new Vector2(towerPozX[towerId], towerPozY[towerId]);
                connectorRendererPrev.transform.localPosition = new Vector2(connectorPozX[towerId], connectorPozY[towerId]);
                gunRendererPrev.transform.localPosition = new Vector2(gunPozX[towerId], gunpozY[towerId]);
                break;
            case "bodies":
                bodyRendererSelect.sprite = bodies[bodyId];
                bodyRendererPrev.sprite = bodies[bodyId];

                bodyRendererSelect.transform.localScale = new Vector2(bodySize[bodyId], bodySize[bodyId]);

                bodyRendererPrev.transform.localScale = new Vector2(bodySize[bodyId], bodySize[bodyId]);
                break;
            case "tracks":
                trackRenderer1Select.sprite = tracks[trackId];
                trackRenderer2Select.sprite = tracks[trackId];
                trackRenderer1Prev.sprite = tracks[trackId];
                trackRenderer2Prev.sprite = tracks[trackId];

                trackRenderer1Select.transform.localPosition = new Vector2(trackPozX1[trackId], trackPozY[trackId]);
                trackRenderer2Select.transform.localPosition = new Vector2(trackPozX2[trackId], trackPozY[trackId]);

                trackRenderer1Prev.transform.localPosition = new Vector2(trackPozX1[trackId], trackPozY[trackId]);
                trackRenderer2Prev.transform.localPosition = new Vector2(trackPozX2[trackId], trackPozY[trackId]);
                break;
            case "colors":
                if(ColorUtility.TryParseHtmlString(colors.Values.ElementAt(colorId), out Color color))
                {
                    colorRendererSelect.color = color;
                    colorRendererPrev.color = color;
                }
                break;
        }
    }
}

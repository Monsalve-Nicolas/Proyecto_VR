using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] Turret turret;
    [SerializeField] TankSpriteModifier spriteModifier;
    [SerializeField] AttackTurret attackTurret;
    public List<StatInfo> currentStats = new List<StatInfo>();
    [Header("Current Piece")]
    public TankPieceScriptable[] piecesArr = new TankPieceScriptable[7];
    StatType[] statTypesArr = new StatType[6];
    TankPieceType[] tankType = new TankPieceType[6]; 
    public Color piece_LightColor;
    public string playerName;
    public int currentDmg;
    public int score;

    private void Start()
    {
        UpdateControllerWithTankPiece();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            LoadData();
        }
    }
    public void OnTankPieceChange(TankPieceScriptable tankPiece)
    {
        piecesArr[(int)tankPiece.pieceType] = tankPiece;

        Debug.Log("Pieza modificada = " + tankPiece.pieceType);
        Debug.Log("El ID = " + tankPiece.id);
        UpdateControllerWithTankPiece();
    }
    public void UpdateControllerWithTankPiece()
    {
        List<StatInfo> statsInfo = new List<StatInfo>();

        foreach (var piece in piecesArr)
        {
            if(piece != null)
            {
                foreach (var item in piece.statInfo)
                {
                    Debug.Log("YEyyyyyy");
                    StatInfo currentStats = statsInfo.Find(x => x.type == item.type);
                    if (currentStats != null)
                    {
                        currentStats.value = item.value;
                    }
                    else
                    {
                        StatInfo newStats = new StatInfo();
                        newStats.value = item.value;
                        newStats.type = item.type;
                        statsInfo.Add(newStats);
                    }
                }
            }

        }
        currentStats = statsInfo;
    }
    public void UpdateControllers()
    {
        foreach (var stats in currentStats)
        {
            switch (stats.type)
            {
                case StatType.Spd:
                    movement.moveSpd += stats.value;
                    break;
                case StatType.RootSpd:
                    movement.rotateSpd += stats.value;
                    turret.rotateSpd += stats.value;
                    break;
                case StatType.Attack:
                    break;
                case StatType.Defense:
                    break;
                case StatType.Life:
                    break;
                case StatType.BulletSpd:
                    break;
                default:
                    break;
            }
        }

    }
    public void LoadData()
    {
        //inicializando el load save
        LoadSaveSystem loadSave = new LoadSaveSystem();
        //obtengo la data
        PlayerDataInfo playerData = loadSave.LoadPlayerInfo();

        //actualizo player con data obtenida
        playerName = playerData.playerName;
        currentDmg = playerData.currentDmg;
        score = playerData.score;

        //inicializo la carga de resource
        LoadResources loadResource = new LoadResources();

        //para cada pieza, estoy cargando el scriptable desde resource, segun tipo e ID
        for (int i = 1; i < piecesArr.Length; i++)
        {
            TankPieceType type = (TankPieceType)i;
            string pieceName = playerData.piecesNames[i - 1];
            Debug.Log("type  = " + type + "/id = " + pieceName);
            piecesArr[i] = loadResource.GetTankPieceScriptable(type, pieceName);
            Debug.Log(piecesArr[i]);
            spriteModifier.ChangeSprite(type, piecesArr[i].pieceSprite);
        }
        UpdateControllerWithTankPiece();
    }
    public void SaveData()
    {
        PlayerDataInfo playerData = new PlayerDataInfo();
        TankPieceScriptable pieceS = new TankPieceScriptable();
        playerData.piecesNames = new List<string>();
        playerData.playerName = playerName;
        playerData.currentDmg = currentDmg;
        playerData.score = score;
        for (int i = 1; i < piecesArr.Length; i++)
        {
            playerData.piecesNames.Add(piecesArr[i].id);
        }
        LoadSaveSystem loadSave = new LoadSaveSystem();
        loadSave.SavePlayerInfo(playerData);
    }
}

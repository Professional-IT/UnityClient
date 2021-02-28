using UnityEngine;

public class TileClickDetector : MonoBehaviour
{
    private TileProperties tileProperties;
    private PawnMover pawnMover;
    public int BoardSize { get; private set; } = 8;



    private void Awake()
    {
        tileProperties = GetComponent<TileProperties>();
        if (PlayerPrefs.HasKey("BoardSize"))
            BoardSize = PlayerPrefs.GetInt("BoardSize");
    }

    private void Start()
    {
        pawnMover = GetComponentInParent<PawnMover>();
    }

    public void ChildPawnClicked()
    {
        OnMouseDown();
    }

    private void OnMouseDown()
    {
        PlayerTurn turn = new PlayerTurn(GetComponent<TileProperties>().GetTileIndex().Index(BoardSize));
        GameManager.Instance.socket.Emit("click", JsonUtility.ToJson(turn));
        if (tileProperties.IsOccupied())
            pawnMover.PawnClicked(tileProperties.GetPawn());
        else
            pawnMover.TileClicked(this.gameObject);
    }

   

    public void ClickTile()
    {
        OnMouseDown();
    }
}
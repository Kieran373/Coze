using UnityEngine;

public class MazeCell : MonoBehaviour
{
    // https://youtu.be/_aeYq5BmDMg?si=FaSx4kuFiwcCUxcp

    [SerializeField]
    private GameObject _frontWall;

    [SerializeField]
    private GameObject _leftWall;

    [SerializeField]
    private GameObject _backWall;

    [SerializeField]
    private GameObject _rightWall;

    [SerializeField]
    private GameObject _unvisitedBlock;

    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
        _unvisitedBlock.SetActive(false);
    }

    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
    }

    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        _backWall.SetActive(false);
    }

    public void ClearRightWall()
    {
        _rightWall.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace My2048
{
    public class GameManager : MonoBehaviour
    {
        public TextMeshProUGUI textScore;
        public TextMeshProUGUI textMoveCount;
        public Button restartBtn;
        public Dictionary<int, GameTile> gameTiles;
        public Dictionary<int, Image> tileImages;

        public HashSet<int> emptyTileIndexSet;

        private InputManager inputManager;
        private bool bTouchDown;
        private Vector3 clickPos;
        
        // Start is called before the first frame update
        void Start()
        {
            inputManager = new InputManager(GameObject.Find("Image_Input").GetComponent<Image>().transform);
            restartBtn = GameObject.Find("Button_Restart").GetComponent<Button>();
            restartBtn.onClick.AddListener(Initialize); // restart 하면 보드판을 초기화 해주는 함수를 호출한다.
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            OnInputHandler();
        }

        void Initialize()
        {
            bTouchDown = false;
            emptyTileIndexSet = new HashSet<int>();
            // 점수판 초기화
            textScore = GameObject.Find("Text_Score").GetComponent<TextMeshProUGUI>();
            textScore.text = "0";
            
            // 이동횟수 초기화
            textMoveCount = GameObject.Find("Text_MoveCount").GetComponent<TextMeshProUGUI>();
            textMoveCount.text = "0";
            
            // 게임타일 초기화
            gameTiles = new Dictionary<int, GameTile>();
            tileImages = new Dictionary<int, Image>();
            for (int i = 0; i < 16; i++)
            {
                emptyTileIndexSet.Add(i);
                // 게임 타일 정보 초기화 후 생성
                GameTile gameTile = new GameTile(i);
                gameTiles.Add(i, gameTile);
                
                // 게임 타일 이미지 초기화 
                Image tileIamge = GameObject.Find($"Image_GameBoard_Fg_Tile_{i}").GetComponent<Image>();
                // - 게임 타일에 적힌 숫자 초기화
                tileIamge.transform.Find("Text_num").GetComponent<TextMeshProUGUI>().text = "";
                // - 게임타일의 컬러 초기화
                Color color = tileIamge.color;
                color.a = 0;
                tileIamge.color = color;
                tileImages.Add(i, tileIamge);
            }
            
            // 최초 2개의 타일 뽑아서 셋팅하기
            // SetTile(0, 2);
            // SetTile(1, 4);
            // SetTile(2, 8);
            // SetTile(3, 16);
            // SetTile(4, 32);
            // SetTile(5, 64);
            // SetTile(6, 128);
            // SetTile(7, 256);
            // SetTile(8, 512);
            // SetTile(9, 1024);
            // SetTile(10, 2048);
            SetTile(SelectTileIndex(), SelectTileNumber());
            SetTile(SelectTileIndex(), SelectTileNumber());
        }

        int SelectTileIndex()
        {
            int selectValue = -1;
            if (emptyTileIndexSet.Count <= 0)
            {
                return selectValue; // 뽑기 실패
            }
            
            int selectIndex = Random.Range(0, emptyTileIndexSet.Count);
            int count = 0;
            foreach (int tileIndex in emptyTileIndexSet)
            {
                if (count == selectIndex)
                {
                    selectValue = tileIndex;
                    break;
                }

                count++;
            }

            if (selectValue >= 0)
            {
                emptyTileIndexSet.Remove(selectValue);
            }
            return selectValue;
        }

        int SelectTileNumber()
        {
            int rand = Random.Range(0, 10); // 0~9 사이 뽑음. 80% 확률로 2, 20% 확률로 4 가 나옴
            if (rand < 8)
            {
                return 2;
            }

            return 4;
        }

        Color GetTileColor(int number)
        {
            if (number == -1)
            {
                return new Color32(0, 0, 0, 0);
            }

            switch (number)
            {
                case 2:
                    return new Color32(238, 238, 219, 255);
                case 4 :
                    return new Color32(238, 223, 198, 255);
                case 8 :
                    return new Color32(242, 178, 116, 255);
                case 16 :
                    return new Color32(245, 149, 98, 255);
                case 32:
                    return new Color32(249, 123, 88, 255);
                case 64:
                    return new Color32(244, 94, 61, 255);
                default :
                    return new Color32(235, 202, 95, 255);
            }
        }

        Color GetNumberColor(int number)
        {
            if (number == -1)
            {
                return new Color32(0, 0, 0, 0);
            }

            switch (number)
            {
                case 2:
                case 4:
                    return new Color32(119, 110, 101, 255);
                default :
                    return new Color32(255, 255, 255, 255);
            }
        }

        void SetTile(int index, int number)
        {
            if (index < 0)
            {
                throw new Exception($"out_of_range_game_tile_index_{index}");
            }
            gameTiles[index].number = number;
            tileImages[index].color = GetTileColor(number);
            tileImages[index].transform.Find("Text_num").GetComponent<TextMeshProUGUI>().color = GetNumberColor(number);
            tileImages[index].transform.Find("Text_num").GetComponent<TextMeshProUGUI>().text = number.ToString();
        }
        
        void OnInputHandler()
        {
            //Touch Down 
            if (!bTouchDown && inputManager.isTouchDown)
            {
                Vector2 point = inputManager.touch2BoardPosition;
                bTouchDown = true;
                clickPos = point;
                //Debug.Log($"Input Down = {point}, local = {inputManager.touch2BoardPosition}");
            }
            //Touch UP : 게임판 영역위에서 마우스 뗌
            else if (bTouchDown && inputManager.isTouchUp)
            {
                Vector2 point = inputManager.touch2BoardPosition;
                Swipe swipeDir = inputManager.EvalSwipDir(clickPos, point);
                //Debug.Log($"Input Up = {point}, local = {inputManager.touch2BoardPosition}");
                Debug.Log($"Swipe : {swipeDir}");
                
                bTouchDown = false;
            }
        }
    }
}

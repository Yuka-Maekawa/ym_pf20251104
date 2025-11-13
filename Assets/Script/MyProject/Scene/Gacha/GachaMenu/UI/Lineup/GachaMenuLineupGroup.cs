using Cysharp.Threading.Tasks;
using MyProject.Gacha.Result;
using UnityEngine;

namespace MyProject.Gacha.Menu
{
    public class GachaMenuLineupGroup : MonoBehaviour
    {
        [SerializeField] private GameObject _baseItemObj = null;

        private static readonly float _viewAlpha = 1f;

        private GameObject[] _itemObjs = null;
        private GachaResultItem[] _resultItems = null;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="itemNum">アイテム数</param>
        public void Initialize(int itemNum)
        {
            _baseItemObj.SetActive(false);

            _itemObjs = new GameObject[itemNum];
            _resultItems = new GachaResultItem[itemNum];
            for (int i = 0; i < itemNum; ++i)
            {
                var obj = Instantiate(_baseItemObj, transform);
                obj.SetActive(true);

                var item = obj.GetComponent<GachaResultItem>();
                item.Initialize();

                _resultItems[i] = item;
                _itemObjs[i] = obj;
            }
        }

        /// <summary>
        /// 解放（非同期）
        /// </summary>
        public async UniTask ReleaseAsync()
        {
            int count = _resultItems.Length;
            for (int i = 0; i < count; ++i)
            {
                await _resultItems[i].ReleaseAsync();
                _resultItems[i] = null;

                Destroy(_itemObjs[i]);
                _itemObjs[i] = null;
            }

            _resultItems = null;
            _itemObjs = null;
        }

        /// <summary>
        /// アイテム情報を設定（非同期）
        /// </summary>
        /// <param name="index">インデックス値</param>
        /// <param name="bgColor">背景色</param>
        /// <param name="texture">サムネイル</param>
        /// <param name="name">アイテム名</param>
        public async UniTask SetupItemAsync(int index, Color bgColor, string textureName, string name)
        {
            if (index < _resultItems.Length)
            {
                var item = _resultItems[index];
                item.SetCanvasGroupAlpha(_viewAlpha);
                await item.SetupItemAsync(bgColor, textureName, name);
            }
        }
    }
}
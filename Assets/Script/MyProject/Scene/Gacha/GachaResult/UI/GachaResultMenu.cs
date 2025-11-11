using Cysharp.Threading.Tasks;
using MyProject.Gacha.Lottery;
using UnityEngine;

namespace MyProject.Gacha.Result
{
    public class GachaResultMenu : MonoBehaviour
    {
        [SerializeField] private Transform _baseParent = null;
        [SerializeField] private GameObject _baseItemObj = null;
        [SerializeField] private Color[] _bgColors = null;

        private GameObject[] _itemObjs = null;
        private GachaResultItem[] _items = null;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="itemNum">アイテム数</param>
        public void Initialize(int itemNum)
        {
            _baseItemObj.SetActive(false);

            _itemObjs = new GameObject[itemNum];
            _items = new GachaResultItem[itemNum];
            for (int i = 0; i < itemNum; ++i)
            {
                var obj = Instantiate(_baseItemObj, _baseParent);
                obj.SetActive(true);

                var item = obj.GetComponent<GachaResultItem>();
                item.Initialize();

                _items[i] = item;
                _itemObjs[i] = obj;
            }
        }

        /// <summary>
        /// 解放(非同期)
        /// </summary>
        public async UniTask ReleaseAsync()
        {
            await UnloadItemsAsync();
        }

        /// <summary>
        /// アイテム情報を開放
        /// </summary>
        public async UniTask UnloadItemsAsync()
        {
            int count = _items.Length;
            for (int i = 0; i < count; ++i)
            {
                await _items[i].ReleaseAsync();
                _items[i] = null;

                Destroy(_itemObjs[i]);
                _itemObjs[i] = null;
            }

            _items = null;
            _itemObjs = null;
        }

        /// <summary>
        /// アイテム設定
        /// </summary>
        public async UniTask SetupItemsAsync(GachaLotteryControllerBase.ItemInfo itemInfo, int index)
        {
            var item = itemInfo.Item;
            await _items[index].SetupItemAsync(_bgColors[(int)itemInfo.Rarity], item.ThumbnailName, item.Name);
        }

        /// <summary>
        /// アイテム設定中か？
        /// </summary>
        /// <returns>true: 設定中, false: 設定終了</returns>
        public bool IsMenuItemsSetting()
        {
            int count = _items.Length;
            for (int i = 0; i < count; ++i)
            {
                if (_items[i].IsSetting)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// アイテムを全て表示
        /// </summary>
        public void ViewAllItem()
        {
            int count = _items.Length;
            for (int i = 0; i < count; ++i)
            {
                ViewItem(i);
            }
        }

        /// <summary>
        /// アイテム表示
        /// </summary>
        /// <param name="index">インデックス値</param>
        private void ViewItem(int index)
        {
            _items[index]?.View();
        }
    }
}
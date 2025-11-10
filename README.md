# Portfolio

## 概要
実務でガチャ機能を実装した経験をもとに、独自の仕様でオリジナルのガチャシステムを作成しました。

※本プロジェクトは短期間で作成したものであり、見た目や演出面は最小限です。  
　コード設計や実装方針の確認を目的としています。

▼デモ

![demo](https://github.com/user-attachments/assets/1fa1d2db-006f-4050-8e4e-576b34ee982f)

---

## 開発環境

| 項目 | 内容 |
|------|------|
| **Unity** | 6000.0.62f1 |
| **バージョン管理** | Git / GitHub |
| **Gitクライアント** | SourceTree |
| **アセット管理** | Addressable Asset System |

**使用理由：**  
実務環境で使用しているツール構成に合わせ、業務との親和性を意識しました。

---

## 開発体制
| 項目 | 内容 |
|------|------|
| **開発人数** | 1人（個人開発） |
| **担当範囲** | 企画 / 設計 / 実装 / デザイン調整 / テスト |

---

## 主な機能
- ガチャ抽選ロジック（確率制御・レアリティ設定対応）  
- Addressableによる動的アセット読み込み  
- ガチャ結果演出（簡易アニメーション付き）  
- データ定義ファイルによる拡張性確保（ScriptableObject使用）  

---


## 工夫した点
- Addressableを用いた **軽量なアセット管理** により、ビルドサイズ削減と柔軟なリソース更新を実現  
- 実務同様に **開発環境を再現**（Git運用、SourceTreeフロー）  
- 短期間でも読みやすいコード設計を意識し、可読性と保守性を重視  

---

## ディレクトリ構成
```
Assets/
├─ _Res/
│ ├─ Common/
│ ├─ Database/
│ │ └─ Gacha/
│ ├─ Scene/
│ │ ├─ Gacha/
│ │ └─ Test/
│ ├─ System/
│ └─ UI/
│   ├─ Gacha/
│   │ └─ GachaResult/
│   │    ├─ Prefab/
│   │    └─ Texture/
│   └─ Test/
├─ AddressableAssetsData/
├─ File/
│ └─ Gacha/
├─ Scripts/
│ ├─ MyProject/
│ │ ├─ Common/
│ │ │ ├─ Lottery/
│ │ │ ├─ Scene/
│ │ │ └─ System/
│ │ ├─ Databse/
│ │ │ └─ Gacha/
│ │ └─ Scene/
│ │   ├─ Gacha/
│ │   │ ├─ GachaMenu/
│ │   │ │ └─ UI/
│ │   │ ├─ GachaResult/
│ │   │ │ └─ UI/
│ │   │ └─ Lottery/
│ │   └─ Test/
│ │     └─ TestGacha/
│ └─ System/
│   ├─ Common/
│   ├─ Editor/
│   │ └─ Addressables/
│   ├─ ObjectBase/
│   └─ ResourceManager/
├─ Setting/
└─ TextMesh Pro/
```


---

## 実行方法
1. Unity 6000.0.62f1 でPortfolioProjectを開く
2. UnityEditorメニューにある MyProject → Build → Resource → Addressable(○○) を選択
3. 2.完了後、同様にUnityEditorメニューにある MyProject → Build → ROM → Release を選択
4.  個人のプロジェクトフォルダ\Builds\Releaseにある「Portfolio_○○_Release」を実行

---

## 今後の拡張予定
- 複数ガチャタイプ（10連・別バージョン）対応
- ラインナップ一覧
- 獲得履歴・ログ機能の追加

---

## 補足
本プロジェクトは、**短期間での開発を通じてコード設計力・実装力を確認できるデモ**として制作しました。  
業務経験で得た知見を反映しつつ、シンプルな構成で基礎技術を整理しています。

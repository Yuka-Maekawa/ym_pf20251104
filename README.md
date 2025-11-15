# Portfolio

## 概要
実務でガチャ機能を実装した経験をもとに、独自の仕様でオリジナルのガチャシステムを作成しました。

※本プロジェクトは短期間で作成したものであり、見た目や演出面は最小限です。  
　コード設計や実装方針の確認を目的としています。

▼デモ

・ガチャ

![Gacha](https://github.com/user-attachments/assets/d68c3f44-4b86-473f-9ce1-c1c7c3ac91fc)

・ガチャシミュレーター

![GachaSimulator](https://github.com/user-attachments/assets/76cb50da-294a-4d9f-ad9c-bbef593c08b7)

---

## 開発環境

| 項目 | 内容 |
|------|------|
| **Unity** | 6000.0.62f1 |
| **バージョン管理** | Git / GitHub |
| **Gitクライアント** | SourceTree |
| **アセット管理** | Addressable Asset System |
| **アニメーション** | DOTween |

**使用理由：**
業務で使用している環境に近い形になるよう、
バージョン管理やアセット管理などを行っております。

---

## 主な機能
- ガチャ抽選ロジック
- Addressableによる動的アセット読み込み  
- DOTweenを使用したアニメーション  
- CSVからScriptableObjectを生成

---

## 工夫した点
- 実務同様に 開発環境を再現（Git運用、SourceTreeフロー）
- Addressableを用いて軽量なアセット管理
- 短期間でも読みやすいコード設計を意識し、可読性と保守性を重視  

---

## ディレクトリ構成

```
Assets/
├─AddressableAssetsData
│  ├─AssetGroups
│  │  └─Schemas
│  ├─AssetGroupTemplates
│  ├─DataBuilders
│  └─Windows
├─File
│  └─Gacha
├─Plugins
│  └─Demigiant
│      └─DOTween
│          ├─Editor
│          │  └─Imgs
│          └─Modules
├─Resources
├─Script
│  ├─MyProject
│  │  ├─Common
│  │  │  ├─Lottery
│  │  │  ├─Scene
│  │  │  ├─System
│  │  │  └─_Common
│  │  │      └─Fade
│  │  ├─Database
│  │  │  └─Gacha
│  │  └─Scene
│  │      ├─Gacha
│  │      │  ├─GachaMenu
│  │      │  │  └─UI
│  │      │  │      └─Lineup
│  │      │  ├─GachaResult
│  │      │  │  └─UI
│  │      │  └─Lottery
│  │      │      ├─Lineup
│  │      │      └─Rarity
│  │      ├─Launcher
│  │      └─Test
│  │          └─TestGacha
│  ├─System
│  │  ├─Common
│  │  ├─Editor
│  │  │  └─Addressables
│  │  ├─ObjectBase
│  │  └─ResourceManager
│  └─Tool
│      └─GachaSimulator
├─Settings
│  └─Build Profiles
├─TextMesh Pro
│  ├─Fonts
│  ├─Resources
│  │  ├─Fonts & Materials
│  │  ├─Sprite Assets
│  │  └─Style Sheets
│  ├─Shaders
│  └─Sprites
└─_Res
    ├─Common
    ├─Database
    │  └─Gacha
    ├─Scene
    │  ├─Gacha
    │  ├─Test
    │  └─Tool
    │      └─GachaSimulator
    ├─System
    │  └─Prefab
    └─UI
        ├─Common
        ├─Gacha
        │  ├─GachaMenu
        │  │  └─Prefab
        │  └─GachaResult
        │      ├─Prefab
        │      └─Texture
        ├─Test
        └─Tool
            └─GachaSimulator
```

---

## 実行方法

▼ガチャ
1. Unity 6000.0.62f1 でPortfolioProjectを開く
2. UnityEditorメニューにある MyProject → Build → Resource → Addressable(○○) を選択
3. 2.完了後、同様にUnityEditorメニューにある MyProject → Build → ROM → Release を選択
4.  個人のプロジェクトフォルダ\Builds\Releaseにある「Portfolio_○○_Release.exe」を実行

▼ガチャシミュレーター
1. Unity 6000.0.62f1 でPortfolioProjectを開く
2. Assets/_Res/Scene/Tool/GachaSimulator/GachaSimulator.unity を起動
3. UnityEditorを実行
4. シミュレーションの設定を行い、スタートボタンを押す
5. D:\"プロジェクト保存先"\Tool\GachaSimulator にシミュレーション結果が保存されています。

---

## 今後の拡張予定
- リファクタリング
- Excelからデータベースを作成
---

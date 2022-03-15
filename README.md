# EFMigrationExample

## EFCore の機能

* ORM

　テーブルごとにデータクラスを定義し、データクラスを操作する事で対応するSQLを生成・実行する機能を提供する仕組み。  
　SQL文を作成・デバッグする事なくDBを扱えるようになる。

* DBマイグレーション

　DBのテーブル構造を世代管理する仕組み。  

## EFCore マイグレーションの流れ

1. モデルクラスを更新する
2. Add-Migration で変更を適用する
    - モデルクラスの更新差分を検出し、DBの構造を変更するコード(Migrations/yyyymmddhh_name.cs) が作成される
3. マイグレーションをDBに適用する
    - DBの構造変更が実際に適用される。
    - ライブラリを呼び出して適用したり、Script-Migration でDB初期化用SQLを生成させて流したりといくつか方法がある。

1~3 を繰り返してDB構造を更新していく。

## 外部キーの切り離し

### 1.初期状態

[モデル](https://github.com/YoshikazuArimitsu/EFMigrateExample/blob/1-INIT/EFMigrateExample/BloggingContext.cs#L29-L42)
[マイグレーション](https://github.com/YoshikazuArimitsu/EFMigrateExample/blob/1-INIT/EFMigrateExample/Migrations/20220315115437_1-Initial.cs)

モデルを定義し、Add-Migration を行うとテーブルを初期化するマイグレーションが生成される。

### 2.モデル更新(親子関係の切り離し)

[モデル更新](https://github.com/YoshikazuArimitsu/EFMigrateExample/blob/2-RemoveBlob_Posts/EFMigrateExample/BloggingContext.cs#L29-L40)
[マイグレーション](https://github.com/YoshikazuArimitsu/EFMigrateExample/blob/2-RemoveBlob_Posts/EFMigrateExample/Migrations/20220315115826_2-Remove_Blog_Posts.cs)

モデルを更新(Blob/Postsの親子関係を削除)し、Add-Migration を行うと、1のテーブル定義からの差分のマイグレーションが生成される。

## NotMapped

EFCoreはモデルの親子関係を検出してある程度自動で外部キー等を張るが、
[NotMapped](https://github.com/YoshikazuArimitsu/EFMigrateExample/blob/3-NotMapped/EFMigrateExample/BloggingContext.cs#L35) アトリビュートを設定されたプロパティは無視される。

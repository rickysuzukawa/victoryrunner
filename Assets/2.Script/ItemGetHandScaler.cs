using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGetHandScaler : MonoBehaviour
{
    public Vector3 enlargedScale = new Vector3(2f, 2f, 2f); // 拡大するスケール
    public float enlargeDuration = 5f; // 拡大の持続時間（秒）
    public float yOffset = 1f; // Y軸方向に移動させる量

    public float blinkDuration = 1f; // 点滅開始から終了までの時間
    public float blinkInterval = 0.1f; // 点滅の間隔
    public Renderer objectRenderer; // ハンドオブジェクトのレンダラー

    private Vector3 originalScale; // 元のスケールを保存
    private float originalYPosition; // 元の位置を保存

    //アイテムゲット時のパーティクル
    [SerializeField] ParticleSystem fireParticle;
    private ParticleSystem itemGetParticle;

    void Start() {
        // 元のスケールと位置を保存
        originalScale = transform.localScale;
        originalYPosition = transform.position.y;

        // レンダラーを取得
        if (objectRenderer == null) {
            Debug.LogError("Rendererが見つかりません。オブジェクトにRendererコンポーネントを追加してください。");
        }

    }

    void OnTriggerEnter(Collider other) {
        // milkアイテムとの衝突を検知
        if (other.CompareTag("ItemBigHand")) {
            // 拡大処理を開始
            StartCoroutine(EnlargeTemporarily());

            // milkアイテムを消去
            Destroy(other.gameObject);
        }
    }

    private void LateUpdate() {

        if (itemGetParticle != null) {

            itemGetParticle.transform.position = this.transform.position;

        }

    }

    IEnumerator EnlargeTemporarily() {

        gameObject.tag = "AttackBigHand";

        // オブジェクトを拡大
        transform.localScale = enlargedScale;

        // オブジェクトの位置をY軸方向にオフセット
        transform.position = new Vector3(
            transform.position.x,
            originalYPosition + yOffset,
            transform.position.z
        );

        itemGetParticle = Instantiate(fireParticle);
        itemGetParticle.Play();

        // 点滅開始まで待機
        yield return new WaitForSeconds(enlargeDuration - blinkDuration);

        // 点滅を開始
        StartCoroutine(BlinkEffect());

        // 点滅終了まで待機
        yield return new WaitForSeconds(blinkDuration);

        // 元のスケールに戻す
        transform.localScale = originalScale;

        // Y軸のみ元に戻し、X軸とZ軸はそのまま
        transform.position = new Vector3(
            transform.position.x,
            originalYPosition,
            transform.position.z
        );

        gameObject.tag = "AttackHand";

        itemGetParticle.Stop();

    }

    IEnumerator BlinkEffect() {
        float elapsed = 0f;
        while (elapsed < blinkDuration) {
            // レンダラーを切り替える
            objectRenderer.enabled = !objectRenderer.enabled;

            // 点滅間隔分待機
            yield return new WaitForSeconds(blinkInterval);

            // 経過時間を加算
            elapsed += blinkInterval;
        }

        // 最後にレンダラーを有効にする
        objectRenderer.enabled = true;
    }
}

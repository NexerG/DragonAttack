using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] List<Sprite> Foto = new List<Sprite>();
    [SerializeField] List<Sprite> Movement = new List<Sprite>();
    [SerializeField] float timer = 0.05f;
    Coroutine m_CoroutineAnim;
    int m_IndexSprite = 0;
    bool isShooting = false;
    bool isMoving = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void PlayAnim()
    {
        //Debug.Log(SceneName);
        m_IndexSprite = 0;
        isShooting = true;
        m_CoroutineAnim = StartCoroutine(Func_PlayShootAnim(Foto));
    }

    internal void PlayMovement()
    {
        isMoving = true;
        if (m_CoroutineAnim != null)
        {
            StopCoroutine(m_CoroutineAnim);
        }
        m_IndexSprite = 0;
        m_CoroutineAnim = StartCoroutine(Func_PlayMovementAnim(Movement));
    }

    internal void StopMovement()
    {
        if (m_CoroutineAnim != null)
            StopCoroutine(m_CoroutineAnim);
        gameObject.GetComponent<SpriteRenderer>().sprite = Movement[0];
        isMoving = false;
    }

    IEnumerator Func_PlayShootAnim(List<Sprite> Foto)
    {
        yield return new WaitForSeconds(timer);
        gameObject.GetComponent<SpriteRenderer>().sprite = Foto[m_IndexSprite];

        if (m_IndexSprite < Foto.Count - 1)
            m_IndexSprite += 1;
        else isShooting = false;
        if (isShooting == true)
            m_CoroutineAnim = StartCoroutine(Func_PlayShootAnim(Foto));
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Foto[0];
        }
    }

    IEnumerator Func_PlayMovementAnim(List<Sprite> Movement)
    {
        yield return new WaitForSeconds(timer);
        gameObject.GetComponent<SpriteRenderer>().sprite = Movement[m_IndexSprite];

        if (m_IndexSprite < Movement.Count - 1)
            m_IndexSprite += 1;
        else m_IndexSprite = 0;
        m_CoroutineAnim = StartCoroutine(Func_PlayMovementAnim(Movement));
    }

}

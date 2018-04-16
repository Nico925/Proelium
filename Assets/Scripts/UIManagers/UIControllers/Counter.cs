using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox
{
    public class Counter : MonoBehaviour
    {
        public Image CounterLable;
        [HideInInspector]
        public Sprite RoundNumber;
        public Sprite Img1;
        public Sprite Img2;
        public Sprite Img3;


        public void DoCountDown()
        {
            CounterLable.sprite = RoundNumber;
            PlayRoundSound(GameManager.Instance.LevelMng.RoundNumber);
            transform.DOScale(new Vector3(1f, 1f, 1f), 2f).OnComplete(() =>
            {
                transform.localScale = Vector3.zero;
                CounterLable.sprite = Img3;
                if (EventManager.OnGameAction != null)
                    EventManager.OnGameAction(AudioManager.AudioInGame.Count3DownAudio);
                transform.DOScale(new Vector3(1f, 1f, 1f), 1f).OnComplete(() =>
                {
                    transform.localScale = Vector3.zero;
                    CounterLable.sprite = Img2;
                    if (EventManager.OnGameAction != null)
                        EventManager.OnGameAction(AudioManager.AudioInGame.Count2DownAudio);
                    transform.DOScale(new Vector3(1f, 1f, 1f), 1f).OnComplete(() =>
                    {
                        transform.localScale = Vector3.zero;
                        CounterLable.sprite = Img1;
                        if (EventManager.OnGameAction != null)
                            EventManager.OnGameAction(AudioManager.AudioInGame.Count1DownAudio);
                        transform.DOScale(new Vector3(1f, 1f, 1f), 1f).OnComplete(() =>
                        {
                            transform.localScale = Vector3.zero;
                            transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).OnComplete(() =>
                            {
                                GameManager.Instance.LevelMng.gameplaySM.CurrentState.OnStateEnd();
                            }).SetEase(Ease.InExpo);
                        }).SetEase(Ease.OutBounce);
                    }).SetEase(Ease.OutBounce);
                }).SetEase(Ease.OutBounce);
            }).SetEase(Ease.OutBounce);
        }

        void PlayRoundSound(int _roundNumber)
        {
            switch (_roundNumber)
            {
                case 1:
                    if (EventManager.OnGameAction != null)
                        EventManager.OnGameAction(AudioManager.AudioInGame.Round1Audio);
                    break;
                case 2:
                    if (EventManager.OnGameAction != null)
                        EventManager.OnGameAction(AudioManager.AudioInGame.Round2Audio);
                    break;
                case 3:
                    if (EventManager.OnGameAction != null)
                        EventManager.OnGameAction(AudioManager.AudioInGame.Round3Audio);
                    break;
                case 4:
                    if (EventManager.OnGameAction != null)
                        EventManager.OnGameAction(AudioManager.AudioInGame.Round4Audio);
                    break;
                case 5:
                    if (EventManager.OnGameAction != null)
                        EventManager.OnGameAction(AudioManager.AudioInGame.Round5Audio);
                    break;
            }
        }
    }
}
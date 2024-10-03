using Soccer;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;

        private IScoreEvents _score;

        [Inject]
        private void Construct(IScoreEvents score)
        {
            _score = score;
        }

        private void Awake()
        {
            RefreshScore(0, 0);
            _score.OnScoreChanged += RefreshScore;
        }

        private void RefreshScore(int red, int blue)
        {
            _text.SetText($"RED {red} : {blue} BLUE");
        }

        private void OnDestroy()
        {
            _score.OnScoreChanged -= RefreshScore;
        }
    }
}

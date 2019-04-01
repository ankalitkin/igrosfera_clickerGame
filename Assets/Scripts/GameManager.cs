using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField, HideInInspector] private Camera _camera;
    [SerializeField, HideInInspector] private Text _scoreText;
    [SerializeField, HideInInspector] private Text _timerText;
    [SerializeField] private GameObject _circle;
    [SerializeField] private GameObject _scoreObject;
    [SerializeField] private GameObject _timerObject;
    [SerializeField] private GameObject _gameOverObject;
    [Space]
    [SerializeField, Min(0)] private float _growthSpeed = 0.1f;
    [SerializeField, Min(0)] private float _decrement = 0.05f;
    [SerializeField, Min(0)] private float _delay = 1f;
    [SerializeField, HideInInspector] private Color _defaultScoreColor;
    private int _score;
    private float _lastClickTime;
    public float GrowthSpeed => _growthSpeed;
    public float Decrement => _decrement;
    public int Score { get { return _score; } }
    private bool _gameStarted = true;
    [Space]
    [SerializeField, Range(0, 1)] private float _saturation = 0.9f;
    [SerializeField, Range(0, 1)] private float _vibrance = 0.9f;
    [SerializeField, Range(0, 1)] private float _offset = 0;
    [SerializeField, Range(1, 360)] private int _nuberOfColors = 12;

    public Color NewColor()
    {
        float hue = (Random.Range(0, _nuberOfColors) + _offset) / _nuberOfColors;
        return Color.HSVToRGB(hue, _saturation, _vibrance);
    }

    private void OnValidate()
    {
        _scoreText = _scoreObject.GetComponent<Text>();
        _timerText = _timerObject.GetComponent<Text>();
        _camera = Camera.main;
        _defaultScoreColor = _scoreText.color;
    }

    private void Start()
    {
        StartCoroutine(Generate(_delay));
        StartCoroutine(UpdateTime());
        Input.simulateMouseWithTouches = false;
        AddScore(0);
    }

    void Update()
    {
        _lastClickTime += Time.deltaTime;

        if (_gameStarted && (Input.GetMouseButton(0) || Input.touchCount > 0))
            _lastClickTime = 0;

        if (!_gameStarted && _lastClickTime > 0.5 && (Input.GetMouseButton(0) || Input.touchCount > 0))
            SceneManager.LoadScene("MenuScene");

        if (_gameStarted)
            ProcessAllCircles();


    }

    public void GameOver()
    {
        _growthSpeed = 0;
        _gameOverObject.SetActive(true);
        _gameStarted = false;
        StopAllCoroutines();
    }

    private void ProcessAllCircles()
    {
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase != TouchPhase.Began) continue;
            var hitInfo = Physics2D.RaycastAll(_camera.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
            if (hitInfo.Length == 0) continue;
            foreach (var hit in hitInfo)
            {
                var cc = hit.transform.gameObject.GetComponent<CircleController>();
                if (cc != null)
                    cc.OnMouseDown();
            }
        }
    }

    public void AddScore(int value)
    {
        _score += value;
        _scoreText.text = "Score: " + _score;
        if (value < 10)
            return;
        _scoreText.color = Color.red;
        DOTween.To(() => _scoreText.color, x => _scoreText.color = x, _defaultScoreColor, 1f);
    }

    private IEnumerator Generate(float time)
    {
        while (true)
        {
            Vector2 viewportPos = new Vector2(Random.Range(0.04f, 0.96f), Random.Range(0.04f, 0.96f));
            if(Physics.Raycast(_camera.ViewportPointToRay(viewportPos)))
                continue;
            Vector2 pos = _camera.ViewportToWorldPoint(viewportPos);
            var circle = Instantiate(_circle, pos, Quaternion.identity).GetComponent<CircleController>();
            circle.Color = NewColor();
            circle.Сontroller = this;
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator UpdateTime()
    {
        while (true)
        {
            _timerText.text = "Time: " + (int)Time.timeSinceLevelLoad;
            yield return new WaitForSeconds(1);
        }
    }
}

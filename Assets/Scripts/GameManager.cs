using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ColorGenerator))]
public class GameManager : MonoBehaviour
{
    [SerializeField, HideInInspector] private Camera _camera;
    [SerializeField, HideInInspector] private ColorGenerator _colorGenerator;
    [SerializeField, HideInInspector] private TextMesh _scoreTextMesh;
    [SerializeField] private GameObject _circle;
    [SerializeField] private GameObject _scoreObject;
    [SerializeField] private GameObject _gameOverObject;
    [SerializeField, Min(0)] private float _growthSpeed = 0.1f;
    [SerializeField, Min(0)] private float _decrement = 0.05f;
    [SerializeField, Min(0)] private float _delay = 1f;
    public float lastClickTime = 0;
    public float GrowthSpeed => _growthSpeed;
    public float Decrement => _decrement;
    public int Score { get; set; }
    private bool gameActive = true;

    private void OnValidate()
    {
        _colorGenerator = GetComponent<ColorGenerator>();
        _scoreTextMesh = _scoreObject.GetComponent<TextMesh>();
        _camera = Camera.main;
    }

    private void Start()
    {
        StartCoroutine(Generate(_delay));
        Input.simulateMouseWithTouches = false;
    }

    void Update()
    {
        lastClickTime += Time.deltaTime;

        if (gameActive && (Input.GetMouseButton(0) || Input.touchCount > 0))
            lastClickTime = 0;

        if (!gameActive && lastClickTime > 0.5 && (Input.GetMouseButton(0) || Input.touchCount > 0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        ProcessAllCircles();

        if (_scoreObject != null)
        {
            Vector3 pos = _camera.ViewportToWorldPoint(new Vector2(0.04f, 0.96f));
            pos = new Vector3(pos.x, pos.y, _scoreObject.transform.position.z);
            _scoreObject.transform.position = pos;
            _scoreTextMesh.text = "Score: " + Score;
        }

        if (_gameOverObject != null && _gameOverObject.activeSelf)
        {
            float s = _camera.ViewportToWorldPoint(new Vector2(1f, 0.5f)).x -
                    _camera.ViewportToWorldPoint(new Vector2(0f, 0.5f)).x;
            _gameOverObject.transform.localScale = Vector3.one * s;
        }
    }

    public void GameOver()
    {
        _growthSpeed = 0;
        _gameOverObject.SetActive(true);
        gameActive = false;
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

    private IEnumerator Generate(float time)
    {
        while (true)
        {
            Vector2 viewportPos = new Vector2(Random.Range(0.04f, 0.96f), Random.Range(0.04f, 0.96f));
            if(Physics.Raycast(_camera.ViewportPointToRay(viewportPos)))
                continue;
            Vector2 pos = _camera.ViewportToWorldPoint(viewportPos);
            var circle = Instantiate(_circle, pos, Quaternion.identity).GetComponent<CircleController>();
            circle.Color = _colorGenerator.NewColor();
            circle.Сontroller = this;
            yield return new WaitForSeconds(time);
        }
    }
}

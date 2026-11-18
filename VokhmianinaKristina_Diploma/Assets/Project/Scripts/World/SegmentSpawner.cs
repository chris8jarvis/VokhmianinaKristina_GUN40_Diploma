using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Project.Player;

namespace Project.World
{
    public class SegmentSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject _segmentPrefab;
        [SerializeField] private int _poolSize = 5; //Сколько сегментов в пуле
        [SerializeField] private float _segmentLength = 20f; //Длина одного сегмента
        [SerializeField] private Transform _player; //Ссылка на игрока

        private List<GameObject> _segmentPool = new List<GameObject>();
        private float _spawnZ = 0f; //Где создаем следующий сегмент
        private SegmentFactory _segmentFactory;

        [Inject]
        public void Construct(PlayerMovement playerMovement, SegmentFactory segmentFactory)
        {
            //Через Zenject получаем доступ к игроку
            _player = playerMovement.transform;
            _segmentFactory = segmentFactory;
            playerMovement.OnPlayerDied += StopGame; 
        }

        private void Start()
        {
            //Создаем пул сегментов заранее (они выключены)
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject segment = Instantiate(_segmentPrefab, transform);
                segment.SetActive(false);
                _segmentPool.Add(segment);
            }

            //Ставим первые сегменты на сцену
            for (int i = 0; i < _poolSize; i++)
            {
                SpawnNextSegment();
            }
        }

        private void Update()
        {
            //Спавним новые сегменты впереди
            if (_player.position.z > _spawnZ - (_poolSize * _segmentLength * 0.5f))
            {
                SpawnNextSegment();
            }

            //Возвращаем старые сегменты в пул
            ReturnOldSegments();
        }

        private void SpawnNextSegment()
        {
            //Ищем неактивный сегмент в пуле
            GameObject segment = _segmentPool.Find(s => !s.activeSelf);

            //Если все сегменты заняты (не хватило пула), создаем новый
            if (segment == null)
            {
                segment = Instantiate(_segmentPrefab, transform);
                _segmentPool.Add(segment);
            }

            segment.transform.position = new Vector3(0, 0, _spawnZ);

            segment.SetActive(true);

             _segmentFactory.PopulateSegment(segment.transform, 0f);

            _spawnZ += _segmentLength;
        }
        private void ReturnOldSegments()
        {
            foreach (GameObject segmentObj in _segmentPool)
            {
                if (segmentObj.activeSelf && segmentObj.transform.position.z < _player.position.z - 30f)
                {
                    //Возвращаем все объекты в пул через скрипт Segment
                    Segment segment = segmentObj.GetComponent<Segment>();
                    if (segment != null)
                    {
                        segment.Clear();
                    }
                    
                    segmentObj.SetActive(false);
                }
            }
        }

        private void StopGame()
        {
            Time.timeScale = 0f;
            Debug.Log("Game over");
        }
    }
}
using System.Collections;
using ie.TUDublin.GE2.Components.Tags;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ie.TUDublin.GE2.Managers {

    public class GameManager : MonoBehaviour {

        [SerializeField] [Min(0)] private int alliedShipsRemaining = 100;
        [SerializeField] [Min(0)] private int enemyShipsRemaining = 100;

        [SerializeField] private int winThreshold = 25;
        [SerializeField] private float resetTime = 3;

        [SerializeField] private TextMeshProUGUI alliedShipCount;
        [SerializeField] private TextMeshProUGUI enemyShipCount;
        [SerializeField] private TextMeshProUGUI winnerText;

        private EntityManager _manager;
        private EntityQuery _alliedShipQuery;
        private EntityQuery _enemyShipQuery;
        
        private bool _isFightOver = false;
        private bool _isReloadSet = false;
        
        private float _reloadTime;

        private void Start() {
            _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            _alliedShipQuery = _manager.CreateEntityQuery(ComponentType.ReadOnly<AlliedTag>());
            _enemyShipQuery = _manager.CreateEntityQuery(ComponentType.ReadOnly<EnemyTag>());
        }

        private void Update() {
            var alliedShips = _alliedShipQuery.ToEntityArray(Allocator.Temp);
            var enemyShips = _enemyShipQuery.ToEntityArray(Allocator.Temp);
            
            alliedShipsRemaining = alliedShips.Length;
            enemyShipsRemaining = enemyShips.Length;

            alliedShipCount.text = "Allied Ships:" + alliedShipsRemaining;
            enemyShipCount.text = "Enemy Ships:" + enemyShipsRemaining;

            if (alliedShipsRemaining <= winThreshold) {
                winnerText.gameObject.SetActive(true);
                winnerText.text = "Allies Win!";
                _isFightOver = true;
            }
            else if (enemyShipsRemaining <= winThreshold){
                winnerText.gameObject.SetActive(true);
                winnerText.text = "Enemies Win!";
                _isFightOver = true;
            }

            if (_isFightOver && !_isReloadSet) {
                _manager.DestroyAndResetAllEntities();
                _isReloadSet = true;
                _reloadTime = Time.timeSinceLevelLoad;
            }

            if (_isReloadSet && Time.timeSinceLevelLoad >= resetTime + _reloadTime) {
                SceneManager.LoadScene("Scenes/Main");
            }

            alliedShips.Dispose();
            enemyShips.Dispose();
        }
    }

}
using System.Collections;
using EventBus;
using EventChannels;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mechanics
{
    public class CaughtZone : MonoBehaviour
    {
        
        private EventBinding<PaintConsumedEvent> _paintConsumedEventBinding;
        private EventBinding<PlayerCaughtEvent> _playerCaughtEventBinding;
        
        [SerializeField] private LayerMask playerLayerMask;
        [SerializeField] private float caughtRadius = 2f;
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private Transform player;
        [SerializeField] private float speed;
        [SerializeField] private AudioSource alarm;
        private readonly Collider[] _colliders = new Collider[1];
        private bool _isPlayerCaught;
        private void Awake()
        {
        
            _paintConsumedEventBinding = new EventBinding<PaintConsumedEvent>(OnPaintConsumed);
            _playerCaughtEventBinding = new EventBinding<PlayerCaughtEvent>(OnPlayerCaught);
            EventBus<PlayerCaughtEvent>.Register(_playerCaughtEventBinding);
            EventBus<PaintConsumedEvent>.Register(_paintConsumedEventBinding);
            //playerCaughtEventChannel.OnEventRaised += OnPlayerCaught;
            patrolPoints = transform.GetComponentsInChildren<Transform>();
            foreach (var patrolPoint in patrolPoints)
            {
            
                patrolPoint.SetParent(null);
            }
            StartCoroutine(MoveToNewSpot());
        
        }

        private void OnPlayerCaught(PlayerCaughtEvent arg0)
        {
            _isPlayerCaught = true;
        }

        private IEnumerator MoveToNewSpot()
        {
            var patrolPoint = patrolPoints[Random.Range(1, patrolPoints.Length)];
            while (true)
            {
                if(Vector3.Distance(transform.position, patrolPoint.position) > .1f && !_isPlayerCaught)
                    transform.position = Vector3.MoveTowards(transform.position, patrolPoint.position, 5 * Time.deltaTime);
                else if(_isPlayerCaught)
                {
                    transform.position = player.position;
                }
                
                else
                {
                
                    yield return new WaitForSeconds(2);
                    patrolPoint = patrolPoints[Random.Range(1, patrolPoints.Length)];
                
                }
                yield return null;
            }

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, caughtRadius);
        }

        private void OnPaintConsumed(PaintConsumedEvent paintConsumedEvent)
        {
            if(Physics.OverlapSphereNonAlloc(transform.position,caughtRadius,_colliders, playerLayerMask) <= 0) return;
        
            EventBus<PlayerCaughtEvent>.Raise(new PlayerCaughtEvent());
            _isPlayerCaught = true;
            alarm.Play();
        }

        private void OnDisable()
        {
   
           EventBus<PlayerCaughtEvent>.Deregister(_playerCaughtEventBinding);
              EventBus<PaintConsumedEvent>.Deregister(_paintConsumedEventBinding);
            StopCoroutine(MoveToNewSpot());
        }
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Charge")]
public class ChargeAction : Action
{
    private float _timer = 0f;
    private bool _loadCharge = false;
    private bool _charging = false;
    private bool _postCharge = false;
    private Vector3 _destinationPoint;
    private int defaultLayer = (1 << 6) + (1 << 8);
    public override void ActionStart(Enemy enemy)
    {
        defaultLayer = LayerMask.GetMask("Level");
        _loadCharge = false;
        _charging = false;
        _postCharge = false;
        _timer = 0f;
    }
    public override void Act(Enemy enemy)
    {
        _timer -= Time.fixedDeltaTime;

        if (_timer <= 0f)
        {
            if (_loadCharge)
            {
                //attack
                _charging = true;
                enemy.SetTrigger(true);
                enemy.isCharging = true;
                _destinationPoint = enemy.transform.position + (enemy.transform.forward * enemy.settings.chargeDistance);
                _loadCharge = false;
            }
            else if (_postCharge || !_charging)
            {
                _postCharge = false;
                enemy.isCharging = false;
                _timer = enemy.settings.chargeLoadingTime;
                _loadCharge = true;
            }
        }

        if (_loadCharge)
        {
            //var dir = _destinationPoint - enemy.transform.position;
            enemy.transform.LookAt(enemy.target.transform);
        }

        if (_charging)
        {
            var enemyPos = enemy.transform.position;
            //Vector3 dir = enemy.target.transform.position - enemyPos;
            Vector3 dir = _destinationPoint - enemyPos;
            dir.y = 0;
            Debug.DrawRay(enemyPos, dir.normalized * enemy.settings.chargeDistance);
            if (Physics.Raycast(enemyPos, dir.normalized, out var hit, dir.magnitude, defaultLayer, QueryTriggerInteraction.Ignore))
            {
                //if (!hit.collider.gameObject.TryGetComponent<Player>(out _))
                //{
                //probably hit wall so stop before this wall
                bool res = UnityEngine.AI.NavMesh.SamplePosition(hit.point, out var navHit, 10f, UnityEngine.AI.NavMesh.AllAreas);
                if (res)
                {
                    _destinationPoint = navHit.position;
                    _destinationPoint.y = enemy.transform.position.y;
                }
                //}
            }
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, _destinationPoint, enemy.settings.chargeSpeed * Time.fixedDeltaTime);
            if (enemy.transform.position == _destinationPoint)
            {
                _charging = false;
                enemy.SetTrigger(false);
                _postCharge = true;
                _timer = enemy.settings.postChargeCooldown;
            }
        }

        enemy.SetDestination(enemy.transform.position);
        enemy.velocity = Vector3.MoveTowards(enemy.velocity, Vector3.zero, enemy.settings.acceleration * Time.deltaTime);
        enemy.Move(0f);
    }

    public override void ActionEnd(Enemy enemy)
    {
        enemy.isCharging = false;
    }
}
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    //[SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;

    private enum State { Roaming, Chasing, Attacking }
    private State state;
    private EnemyPathfinding1 enemyPathfinding;
    private Transform player;
    private bool canAttack = true;
    private Coroutine roamingCoroutine;
    private Animator animator;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding1>();
        animator = GetComponent<Animator>();
        state = State.Roaming;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the Player has the 'Player' tag.");
        }
    }

    private void Start()
    {
        roamingCoroutine = StartCoroutine(RoamingRoutine());
        animator.SetBool("isMoving", true);  // ✅ Bật animation di chuyển ngay từ đầu
    }

    //private void Update()
    //{
    //    if (player == null) return;

    //    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    //    Debug.Log($"Enemy State: {state}, Distance to Player: {distanceToPlayer}");

    //    if (distanceToPlayer < attackRange && canAttack)
    //    {
    //        if (state != State.Attacking)
    //        {
    //            Debug.Log("Switching to ATTACK state");
    //            SwitchState(State.Attacking);
    //        }
    //    }
    //    else if (distanceToPlayer < detectRange)
    //    {
    //        if (state != State.Chasing)
    //        {
    //            Debug.Log("Switching to CHASING state");
    //            SwitchState(State.Chasing);
    //        }
    //    }
    //    else
    //    {
    //        if (state != State.Roaming)
    //        {
    //            Debug.Log("Switching to ROAMING state");
    //            SwitchState(State.Roaming);
    //        }
    //    }
    //}

    private State lastState; // Lưu trạng thái trước đó

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRange && canAttack)
        {
            if (state != State.Attacking)
            {
                SwitchState(State.Attacking);
            }
        }
        else if (distanceToPlayer < detectRange)
        {
            if (state != State.Chasing)
            {
                SwitchState(State.Chasing);
            }
        }
        else
        {
            if (state != State.Roaming)
            {
                SwitchState(State.Roaming);
            }
        }

        // Chỉ in log khi trạng thái thay đổi
        if (state != lastState)
        {
            Debug.Log($"Enemy State: {state}, Distance to Player: {distanceToPlayer}");
            lastState = state; // Cập nhật trạng thái mới
        }
    }

    private void SwitchState(State newState)
    {
        if (state == newState) return;

        state = newState;
        Debug.Log($"State changed to: {state}");

        if (state == State.Roaming)
        {
            if (roamingCoroutine == null)
            {
                roamingCoroutine = StartCoroutine(RoamingRoutine());
            }
            enemyPathfinding.MoveTo(GetRoamingPosition());
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            if (roamingCoroutine != null)
            {
                StopCoroutine(roamingCoroutine);
                roamingCoroutine = null;
            }

            if (state == State.Chasing)
            {
                Debug.Log("Enemy is chasing the player!");
                animator.SetBool("isMoving", true);
                enemyPathfinding.MoveTo(player.position); // ✅ Kiểm tra xem enemy có di chuyển không
            }
            else if (state == State.Attacking)
            {
                enemyPathfinding.StopMoving();
                animator.SetBool("isMoving", false);
                animator.SetTrigger("Attack");
                animator.SetBool("isAttacking", true);
                StartCoroutine(AttackPlayer());
            }
        }
    }


    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(roamChangeDirFloat);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private IEnumerator AttackPlayer()
    {
        canAttack = false;
        Debug.Log("Enemy attacks player!");

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
        animator.SetBool("isAttacking", false);  // ✅ Đảm bảo animation attack dừng lại sau cooldown
    }
}
//using System.Collections;
//using UnityEngine;

//public class EnemyMovement : MonoBehaviour
//{
//    [SerializeField] private float roamChangeDirFloat = 2f; // Thời gian thay đổi hướng di chuyển của kẻ thù khi đang lang thang
//    [SerializeField] private float chaseSpeed = 3f; // Tốc độ khi kẻ thù đuổi theo người chơi
//    [SerializeField] private float detectRange = 5f; // Khoảng cách phát hiện người chơi
//    [SerializeField] private float attackRange = 1f; // Khoảng cách tấn công của kẻ thù
//    [SerializeField] private float attackCooldown = 1f; // Thời gian chờ giữa các lần tấn công

//    private enum State { Roaming, Chasing, Attacking } // Các trạng thái của kẻ thù: lang thang, đuổi theo, tấn công
//    private State state; // Biến lưu trạng thái hiện tại của kẻ thù
//    private EnemyPathfinding1 enemyPathfinding; // Đối tượng để điều khiển di chuyển của kẻ thù
//    private Transform player; // Biến lưu đối tượng người chơi
//    private bool canAttack = true; // Biến kiểm tra liệu kẻ thù có thể tấn công không
//    private Coroutine roamingCoroutine; // Biến lưu coroutine cho hành động lang thang
//    private Animator animator; // Biến lưu animator để điều khiển các hoạt động hoạt hình của kẻ thù

//    private void Awake()
//    {
//        enemyPathfinding = GetComponent<EnemyPathfinding1>(); // Lấy component EnemyPathfinding1 để điều khiển di chuyển
//        animator = GetComponent<Animator>(); // Lấy component Animator để điều khiển hoạt hình của kẻ thù
//        state = State.Roaming; // Ban đầu kẻ thù ở trạng thái lang thang

//        GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); // Tìm đối tượng người chơi bằng thẻ "Player"
//        if (playerObj != null)
//        {
//            player = playerObj.transform; // Nếu tìm thấy người chơi, gán biến player
//        }
//        else
//        {
//            Debug.LogError("Player not found! Make sure the Player has the 'Player' tag."); // Thông báo lỗi nếu không tìm thấy người chơi
//        }
//    }

//    private void Start()
//    {
//        roamingCoroutine = StartCoroutine(RoamingRoutine()); // Bắt đầu coroutine để thực hiện hành động lang thang
//        animator.SetBool("isMoving", true);  // ✅ Bật animation di chuyển ngay từ đầu
//    }

//    private void Update()
//    {
//        if (player == null) return; // Nếu không tìm thấy người chơi thì không làm gì cả

//        float distanceToPlayer = Vector2.Distance(transform.position, player.position); // Tính khoảng cách từ kẻ thù đến người chơi

//        Debug.Log($"Enemy State: {state}, Distance to Player: {distanceToPlayer}"); // In ra trạng thái của kẻ thù và khoảng cách đến người chơi

//        if (distanceToPlayer < attackRange && canAttack) // Nếu khoảng cách đến người chơi nhỏ hơn phạm vi tấn công và có thể tấn công
//        {
//            if (state != State.Attacking) // Nếu không ở trạng thái tấn công
//            {
//                Debug.Log("Switching to ATTACK state"); // Đổi trạng thái thành tấn công
//                SwitchState(State.Attacking);
//            }
//        }
//        else if (distanceToPlayer < detectRange) // Nếu khoảng cách đến người chơi nhỏ hơn phạm vi phát hiện
//        {
//            if (state != State.Chasing) // Nếu không ở trạng thái đuổi theo
//            {
//                Debug.Log("Switching to CHASING state"); // Đổi trạng thái thành đuổi theo
//                SwitchState(State.Chasing);
//            }
//        }
//        else // Nếu không ở trong phạm vi phát hiện hay tấn công
//        {
//            if (state != State.Roaming) // Nếu không ở trạng thái lang thang
//            {
//                Debug.Log("Switching to ROAMING state"); // Đổi trạng thái thành lang thang
//                SwitchState(State.Roaming);
//            }
//        }
//    }


//    private void SwitchState(State newState)
//    {
//        if (state == newState) return; // Nếu trạng thái mới giống với trạng thái hiện tại thì không làm gì

//        state = newState; // Đổi trạng thái thành trạng thái mới
//        Debug.Log($"State changed to: {state}"); // In ra trạng thái mới

//        if (state == State.Roaming) // Nếu trạng thái là lang thang
//        {
//            if (roamingCoroutine == null) // Nếu chưa có coroutine lang thang nào
//            {
//                roamingCoroutine = StartCoroutine(RoamingRoutine()); // Bắt đầu coroutine lang thang
//            }
//            enemyPathfinding.MoveTo(GetRoamingPosition()); // Di chuyển kẻ thù đến vị trí lang thang
//            animator.SetBool("isMoving", true); // Bật animation di chuyển
//            animator.SetBool("isAttacking", false); // Tắt animation tấn công
//        }
//        else
//        {
//            if (roamingCoroutine != null) // Nếu có coroutine lang thang
//            {
//                StopCoroutine(roamingCoroutine); // Dừng coroutine lang thang
//                roamingCoroutine = null;
//            }

//            if (state == State.Chasing) // Nếu trạng thái là đuổi theo
//            {
//                Debug.Log("Enemy is chasing the player!"); // In ra thông báo kẻ thù đang đuổi theo người chơi
//                animator.SetBool("isMoving", true); // Bật animation di chuyển
//                enemyPathfinding.MoveTo(player.position); // Di chuyển kẻ thù đến vị trí của người chơi
//            }
//            else if (state == State.Attacking) // Nếu trạng thái là tấn công
//            {
//                enemyPathfinding.StopMoving(); // Dừng di chuyển của kẻ thù
//                animator.SetBool("isMoving", false); // Tắt animation di chuyển
//                animator.SetTrigger("Attack"); // Kích hoạt animation tấn công
//                animator.SetBool("isAttacking", true); // Bật trạng thái tấn công trong animation
//                StartCoroutine(AttackPlayer()); // Bắt đầu coroutine tấn công
//            }
//        }
//    }


//    private IEnumerator RoamingRoutine()
//    {
//        while (state == State.Roaming) // Khi kẻ thù đang trong trạng thái lang thang
//        {
//            Vector2 roamPosition = GetRoamingPosition(); // Lấy vị trí lang thang ngẫu nhiên
//            enemyPathfinding.MoveTo(roamPosition); // Di chuyển kẻ thù đến vị trí lang thang
//            yield return new WaitForSeconds(roamChangeDirFloat); // Chờ một khoảng thời gian trước khi thay đổi hướng
//        }
//    }

//    private Vector2 GetRoamingPosition()
//    {
//        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // Trả về một vị trí ngẫu nhiên trong phạm vi nhất định
//    }

//    private IEnumerator AttackPlayer()
//    {
//        canAttack = false; // Đặt trạng thái không thể tấn công trong khi đang thực hiện hành động tấn công
//        Debug.Log("Enemy attacks player!"); // In ra thông báo kẻ thù đang tấn công

//        yield return new WaitForSeconds(attackCooldown); // Chờ thời gian cooldown giữa các lần tấn công

//        canAttack = true; // Sau khi cooldown, kẻ thù có thể tấn công lại
//        animator.SetBool("isAttacking", false);  // ✅ Đảm bảo animation attack dừng lại sau cooldown
//    }
//}

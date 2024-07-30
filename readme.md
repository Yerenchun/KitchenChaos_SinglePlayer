# 胡闹厨房单机版

## 前提设置

### URP通用管线项目设置

删除其他质量，保留高质量

### 代码风格

```c#
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyCodeStyle : MonoBehaviour
{
    // Constants: UpperCase SnakeCase
    public const int CONSTANT_FIELD = 56;

    // Properties: PascalCase
    public static MyCodeStyle Instance { get; private set; }

    // Events: PascalCase
    public event EventHandler OnSomethingHappened;

    // Fields: camelCase
    private float memberVariable;

    // Function Names: PascalCase
    private void Awake()
    {
        Instance = this;

        DoSomething(10f);
    }

    // Function Params: camelCase
    private void DoSomething(float time){
        // Do something...
        memberVariable = time + Time.deltaTime;
        if (memberVariable > 0){
            // Do something else...
        }
    }
}
```

### 导入资产后处理

使用的是URP通用渲染管线模版

删除自带的全局效果，新建一个新的全局效果

**后续可以通过重写，得到想要的全局效果**





添加地板，修改为使用资产中的`Floor`材质



并且放上去几个模型。

用来查看全局效果。





#### 重写全局效果

1. 添加一个中性色调

   

2. 添加色彩调整

   设置20的对比度和20的饱和度

   

3. 添加Bloom效果

   为了让炉子有着火的效果

   添加一个Bloom效果

   阈值和强度可以根据自己的情况来调整

   

4. 添加镜头边缘暗角效果

   当强度设置得很高时，就是这种效果

   

   不过实际上，只需要将强度设置为0.25就够了

5. 可以在URP中设置抗锯齿的精度

   

   也可以在摄像机中，设置抗锯齿的模式

   

6. 设置URP渲染设置中的`Screen Space Ambient Occlusion`用来增强场景的深度感和细节，使物体之间的阴影和光线过渡更加自然。

   即，模拟环境光遮蔽

   

   



7. 设置摄像机的角度

   

8. 可以自己设置光照等相关效果

   



## 角色控制器、角色视觉，旋转、动画



**将视觉效果（人物模型）与逻辑分开**

创建一个空物体，并且重置坐标，作为`Player`的父物体，然后在这个空物体中，再创建真正的人物





这样子，父物体，不需要任何`y`轴上的补偿，就可以直接控制人物移动了

并且父物体，可以附加所有的逻辑组件

而我们可以通过替换子物体，来改变`Player`的视觉效果，即，人物的模型。

也就是说，将人物的主要逻辑和人物的模型分开来。



创建`Player`脚本，挂载到`Player`父物体上

控制脚本中，**将输入，和实际的移动逻辑分离开来**

这有助于，后续重构代码，使用新的输入系统

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private float moveSpeed = 7f;

    private void Update() {
        Vector2 inputVector = Vector2.zero;

        if(Input.GetKey(KeyCode.W)){
            inputVector.y = +1;
        }
        if(Input.GetKey(KeyCode.A)){
            inputVector.x = -1;
        }
        if(Input.GetKey(KeyCode.S)){
            inputVector.y = -1;
        }
        if(Input.GetKey(KeyCode.D)){
            inputVector.x = +1;
        }
        inputVector.Normalize();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position += moveDir * Time.deltaTime * moveSpeed;
        Debug.Log(inputVector);

    }
}
```





使用预设体，替换原来的胶囊体，将原来的胶囊体删除掉





**这里使用了一种比较特殊的控制人物旋转的方法**

**没有使用四元数和欧拉角**

通过设置人物的面朝向，来控制人物的旋转方向。

并且使用插值函数，来让旋转变得丝滑

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;

    private void Update() {
        Vector2 inputVector = Vector2.zero;

        if(Input.GetKey(KeyCode.W)){
            inputVector.y = +1;
        }
        if(Input.GetKey(KeyCode.A)){
            inputVector.x = -1;
        }
        if(Input.GetKey(KeyCode.S)){
            inputVector.y = -1;
        }
        if(Input.GetKey(KeyCode.D)){
            inputVector.x = +1;
        }
        inputVector.Normalize();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        // 通过设置自己的面朝向，来设置自己的旋转方向
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        // transform.forward = moveDir;

    }
}
```



创建角色的动画状态机

为了控制角色的动画，单独创建一个脚本`PlayerAnimator`

注意，这个脚本，要挂载在人物模型物体上面

通过在`Player`中，给外部传递一个是否行走的状态

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    private bool isWalking;

    private void Update() {
        Vector2 inputVector = Vector2.zero;

        if(Input.GetKey(KeyCode.W)){
            inputVector.y = +1;
        }
        if(Input.GetKey(KeyCode.A)){
            inputVector.x = -1;
        }
        if(Input.GetKey(KeyCode.S)){
            inputVector.y = -1;
        }
        if(Input.GetKey(KeyCode.D)){
            inputVector.x = +1;
        }
        inputVector.Normalize();

        // 设置移动的方向
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        transform.position += moveDir * Time.deltaTime * moveSpeed;

        isWalking = moveDir != Vector3.zero;

        // 通过设置自己的面朝向，来设置自己的旋转方向
        // transform.forward = moveDir;
        // 通过插值函数，使旋转更加平滑
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

    public bool IsWalking(){
        return isWalking;
    }
}
```



在`PlayerAnimator`脚本中，控制人物的动画播放

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    // 避免使用的动画名称错误
    private const string IS_WALKING = "IsWalking";
    [SerializeField] private Player player;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
```









 














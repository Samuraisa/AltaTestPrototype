using UnityEngine;

public class GizmosDrawer : MonoBehaviour
{
    private enum GizmoType
    {
        Sphere = 0,
        WireSphere = 1,
        Cube = 2,
        WireCube = 3,
        Mesh = 4,
        WireMesh = 5,
        Line = 6,
        Ray = 7,
        Capsule = 8
    }

    private readonly struct GizmoInfo
    {
        public readonly GizmoType type;
        public readonly float radius;
        public readonly Mesh mesh;
        public readonly Color color;
        public readonly float time;

        private readonly Vector3 vStart;
        private readonly Vector3 vEnd;

        public GizmoInfo(GizmoType gizmoType, Vector3 point1, Vector3 point2, Color c, float t = 0f)
        {
            type = gizmoType;
            radius = 0f;
            mesh = null;
            color = c;
            vStart = point1;
            vEnd = point2;
            time = GetTime(t);
        }

        public GizmoInfo(GizmoType gizmoType, Vector3 point1, Vector3 point2, float r, Color c, float t = 0f)
        {
            type = gizmoType;
            radius = r;
            mesh = null;
            color = c;
            vStart = point1;
            vEnd = point2;
            time = GetTime(t);
        }

        public GizmoInfo(GizmoType gizmoType, Vector3 point, float r, Color c, float t = 0f)
        {
            type = gizmoType;
            radius = r;
            mesh = null;
            color = c;
            vStart = point;
            vEnd = Vector3.zero;
            time = GetTime(t);
        }

        public GizmoInfo(GizmoType gizmoType, Mesh m, Color c, float t = 0f)
        {
            type = gizmoType;
            radius = 0f;
            mesh = m;
            color = c;
            vStart = Vector3.zero;
            vEnd = Vector3.zero;
            time = GetTime(t);
        }

        private static float GetTime(float value)
        {
            return Time.time + (value > 0f ? value : Time.deltaTime);
        }

        public Vector3 center => vStart;
        public Vector3 start => vStart;
        public Vector3 end => vEnd;
        public Vector3 size => vEnd;
        public Vector3 direction => vEnd;
    }

    private static readonly Vector3[] s_capsuleOffsets = new Vector3[]
    {
        Vector3.forward, Vector3.back, Vector3.left, Vector3.right
    };

    private static Color s_color = Color.white;
    private const int MaxGizmos = 3000;
    private static GizmoInfo[] s_drawList = new GizmoInfo[MaxGizmos];
    private static int s_num = 0;

    public static void DrawSphere(Vector3 center, float radius, float time = 0f)
    {
        DrawSphere(center, radius, s_color, time);
    }

    public static void DrawSphere(Vector3 center, float radius, Color color, float time = 0f)
    {
        if (s_num < s_drawList.Length)
        {
            s_drawList[s_num] = new GizmoInfo(GizmoType.Sphere, center, radius, color, time);
            s_num++;
        }
    }

    public static void DrawWireSphere(Vector3 center, float radius, float time = 0f)
    {
        DrawWireSphere(center, radius, s_color, time);
    }

    public static void DrawWireSphere(Vector3 center, float radius, Color color, float time = 0f)
    {
        if (s_num < s_drawList.Length)
        {
            s_drawList[s_num] = new GizmoInfo(GizmoType.WireSphere, center, radius, color, time);
            s_num++;
        }
    }

    public static void DrawCube(Vector3 center, Vector3 size, float time = 0f)
    {
        DrawCube(center, size, s_color, time);
    }

    public static void DrawCube(Vector3 center, Vector3 size, Color color, float time = 0f)
    {
        if (s_num < s_drawList.Length)
        {
            s_drawList[s_num] = new GizmoInfo(GizmoType.Cube, center, size, color, time);
            s_num++;
        }
    }

    public static void DrawWireCube(Vector3 center, Vector3 size, float time = 0f)
    {
        DrawWireCube(center, size, s_color, time);
    }

    public static void DrawWireCube(Vector3 center, Vector3 size, Color color, float time = 0f)
    {
        if (s_num < s_drawList.Length)
        {
            s_drawList[s_num] = new GizmoInfo(GizmoType.WireCube, center, size, color, time);
            s_num++;
        }
    }

    public static void DrawMesh(Mesh mesh, float time = 0f)
    {
        DrawMesh(mesh, s_color, time);
    }

    public static void DrawMesh(Mesh mesh, Color color, float time = 0f)
    {
        if (s_num < s_drawList.Length)
        {
            s_drawList[s_num] = new GizmoInfo(GizmoType.Mesh, mesh, color, time);
            s_num++;
        }
    }

    public static void DrawWireMesh(Mesh mesh, float time = 0f)
    {
        DrawWireMesh(mesh, s_color, time);
    }

    public static void DrawWireMesh(Mesh mesh, Color color, float time = 0f)
    {
        if (s_num < s_drawList.Length)
        {
            s_drawList[s_num] = new GizmoInfo(GizmoType.WireMesh, mesh, color, time);
            s_num++;
        }
    }

    public static void DrawLine(Vector3 start, Vector3 end, float time = 0f)
    {
        DrawLine(start, end, s_color, time);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float time = 0f)
    {
        if (s_num < s_drawList.Length)
        {
            s_drawList[s_num] = new GizmoInfo(GizmoType.Line, start, end, color, time);
            s_num++;
        }
    }

    public static void DrawRay(Vector3 start, Vector3 direction, float time = 0f)
    {
        DrawRay(start, direction, s_color, time);
    }

    public static void DrawRay(Vector3 start, Vector3 direction, Color color, float time = 0f)
    {
        if (s_num < s_drawList.Length)
        {
            s_drawList[s_num] = new GizmoInfo(GizmoType.Ray, start, direction, color, time);
            s_num++;
        }
    }

    public static void DrawCapsule(Vector3 start, Vector3 end, float radius, Color color, float time = 0f)
    {
        if (s_num < s_drawList.Length)
        {
            s_drawList[s_num] = new GizmoInfo(GizmoType.Capsule, start, end, radius, color, time);
            s_num++;
        }
    }

    public static void DrawCapsule(Vector3 start, Vector3 end, float radius, float time = 0f)
    {
        if (s_num < s_drawList.Length)
        {
            s_drawList[s_num] = new GizmoInfo(GizmoType.Capsule, start, end, radius, s_color, time);
            s_num++;
        }
    }

#if !UNITY_EDITOR
        private void Awake()
        {
            Destroy(this);
        }
#endif

    private void OnDisable()
    {
        s_num = 0;
    }

    void OnDrawGizmos()
    {
        if (s_num > 0)
        {
            int shift = 0;
            for (int i = 0; i < s_num; i++)
            {
                DrawGizmo(in s_drawList[i]);

                if (s_drawList[i].time < Time.time)
                {
                    ++shift;
                }
                else
                {
                    if (shift > 0)
                    {
                        s_drawList[i - shift] = s_drawList[i];
                    }
                }
            }

            s_num -= shift;
            s_color = Color.white;
        }
    }

    void DrawGizmo(in GizmoInfo g)
    {
        Gizmos.color = g.color;

        switch (g.type)
        {
            case GizmoType.Sphere:
                Gizmos.DrawSphere(g.center, g.radius);
                break;

            case GizmoType.WireSphere:
                Gizmos.DrawWireSphere(g.center, g.radius);
                break;

            case GizmoType.Cube:
                Gizmos.DrawCube(g.center, g.size);
                break;

            case GizmoType.WireCube:
                Gizmos.DrawWireCube(g.center, g.size);
                break;

            case GizmoType.Mesh:
                Gizmos.DrawMesh(g.mesh);
                break;

            case GizmoType.WireMesh:
                Gizmos.DrawWireMesh(g.mesh);
                break;

            case GizmoType.Line:
                Gizmos.DrawLine(g.start, g.end);
                break;

            case GizmoType.Ray:
                Gizmos.DrawRay(g.start, g.direction);
                break;

            case GizmoType.Capsule:
                Gizmos.DrawWireSphere(g.start, g.radius);
                Gizmos.DrawWireSphere(g.end, g.radius);
                Gizmos.DrawLine(g.start, g.end);

                var capsuleRotation = Quaternion.LookRotation(g.end - g.start);
                for (int i = 0; i < s_capsuleOffsets.Length; i++)
                {
                    var offset = capsuleRotation * (s_capsuleOffsets[i] * g.radius);
                    Gizmos.DrawLine(g.start + offset, g.end + offset);
                }

                break;
        }
    }
}
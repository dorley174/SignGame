using System.Collections;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer brushPrefab;
    public Camera cam;
    public float minDistanceForNewPoint = 0.1f;
    public float secondsForLineToLive = 3f;

    LineRenderer currentLine;

    void Update() {
        Draw();
    }

    void Draw() {
        if (Input.GetMouseButtonDown(0)) {
            currentLine = Instantiate(brushPrefab);
            currentLine.transform.SetParent(transform, false);
            currentLine.SetPosition(0, GetMouseLocalPos());
        } else if (Input.GetMouseButton(0)) {
            if (DistanceToLastPoint(GetMouseLocalPos()) < minDistanceForNewPoint) return;
            ++currentLine.positionCount;
            int posIndex = currentLine.positionCount - 1;
            currentLine.SetPosition(posIndex, GetMouseLocalPos());
        } else if (Input.GetMouseButtonUp(0)) {
            StartCoroutine(RemoveLineAfter(currentLine, secondsForLineToLive));
            currentLine = null;
        }
    }

    Vector3 GetMouseLocalPos() {
        Vector3 mouseDrawerPos = GetMouseWorldPos();
        Vector3 localPos = transform.InverseTransformPoint(mouseDrawerPos);
        return localPos;
    }

    float DistanceToLastPoint(Vector3 pos) {
        if (currentLine == null) return 0;
        return Vector3.Distance(currentLine.GetPosition(currentLine.positionCount - 1), pos);
    }

    IEnumerator RemoveLineAfter(LineRenderer line, float secondsToWait) {
        yield return new WaitForSeconds(secondsToWait);
        Destroy(line.gameObject);
    }

    public Vector3 GetMouseWorldPos() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane xy = new(Vector3.forward, new Vector3(0, 0, transform.position.z));
        xy.Raycast(ray, out float distance);
        return ray.GetPoint(distance);
    }
}

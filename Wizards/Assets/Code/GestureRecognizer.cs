using UnityEngine;
using System.Collections;
using System.IO;

public class GestureRecognizer : MonoBehaviour
{
    // recognizer settings
    int maxPoints = 128;					// max number of point in the gesture
    int sizeOfScaleRect = 500;			// the size of the bounding box

    //tweak the precision of gesture recognizing by changing the factor for the distance between points or the scoreThreshold
    float factor = 1f;
    float scoreThreshold = 0.7f;         // Minimum score needed for a gesture match

    public  ArrayList TemplateNames = new ArrayList();
    public  ArrayList Templates = new ArrayList();
    public SpellGenerator sg;
    public ParticleSystem ps;
    public GameObject drawCam;

    public  void loadGestures()
    {
        //Read all gestures from file
        using (StreamReader reader = new StreamReader(Application.dataPath + "/Resources/Gestures" + maxPoints + ".txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                ArrayList temp = new ArrayList();
                string[] words = line.Split(';');
                for (int i = 1; i < words.Length; i++)
                {
                    string[] vec = words[i].Split(',');
                    temp.Add(new Vector2(float.Parse(vec[0]), float.Parse(vec[1])));
                }

                TemplateNames.Add(words[0]);
                Templates.Add(temp);
            }
        }
    }

    /// <summary>
    /// Function to generate all the basic gestures used for comparing
    /// </summary>
    public void generateGestures()
    {
        // Fire1 gesture
        ArrayList Fire1 = new ArrayList();
        for (int i = 0; i < maxPoints; i++)
        {
            Fire1.Add(new Vector2(0 + (i / 2), 0 + i));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Fire1.Add(new Vector2((maxPoints / 2) + (i / 2), maxPoints - i));
        }
        recordTemplate(Fire1, "0");

        // Fire2 gesture
        ArrayList Fire2 = new ArrayList();
        for (int i = 0; i < maxPoints; i++)
        {
            Fire2.Add(new Vector2(0 + (i / 2), 0 + i));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Fire2.Add(new Vector2((maxPoints / 2) + (i / 2), maxPoints - i));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Fire2.Add(new Vector2(maxPoints + (i / 2), 0 + i));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Fire2.Add(new Vector2(maxPoints + (maxPoints / 2) + (i / 2), maxPoints - i));
        }
        recordTemplate(Fire2, "1");

        // Fire3 gesture
        ArrayList Fire3 = new ArrayList();
        for (int i = 15; i < maxPoints; i++)
        {
            Fire3.Add(new Vector2(0 + i, 0 + i));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Fire3.Add(new Vector2(maxPoints + i, maxPoints - i));
        }
        for (int i = 0; i < maxPoints * 2; i++)
        {
            Fire3.Add(new Vector2((maxPoints * 2) - i, 0));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Fire3.Add(new Vector2(0 + (i / 2), 0 - i));
        }
        for (int i = 0; i < maxPoints - 10; i++)
        {
            Fire3.Add(new Vector2((maxPoints / 2) + ((i * (128f / 118f)) / 2), (0 - maxPoints) + i));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Fire3.Add(new Vector2(maxPoints + (i / 2), -10 - i));
        }
        for (int i = 0; i < maxPoints - 10; i++)
        {
            Fire3.Add(new Vector2(maxPoints + (maxPoints / 2) + (i / 2), (0 - maxPoints) + i));
        }
        recordTemplate(Fire3, "2");

        // Air1 gesture
        ArrayList Air1 = new ArrayList();
        for (float i = 120; i < 420; i++)
        {
            Air1.Add(new Vector2(Mathf.Sin(i * Mathf.PI / 180.0f) * 500, Mathf.Cos(i * Mathf.PI / 180.0f) * 500));
        }
        recordTemplate(Air1, "3");

        // Air2 gesture
        ArrayList Air2 = new ArrayList();
        for (int i = 600; i >= 0; i--)
        {
            Air2.Add(new Vector2((Mathf.Sin(30 * Mathf.PI / 180.0f) * 970) + i, (-Mathf.Cos(30 * Mathf.PI / 180.0f) * 970) - i));
        }
        for (float i = 30; i < 660; i++)
        {
            Air2.Add(new Vector2(Mathf.Sin(i * Mathf.PI / 180.0f) * (1000 - i), -Mathf.Cos(i * Mathf.PI / 180.0f) * (1000 - i)));
        }
        recordTemplate(Air2, "4");

        // Air3 gesture
        ArrayList Air3 = new ArrayList();
        for (float i = 180; i < 1080; i++)
        {
            Air3.Add(new Vector2((Mathf.Sin(i * Mathf.PI / 180.0f) * 1000) - (2f * i), (Mathf.Cos(i * Mathf.PI / 180.0f) * 1000) + (5f * i)));
        }
        recordTemplate(Air3, "5");

        // Water1 gesture
        ArrayList Water1 = new ArrayList();
        for (float i = 120; i < 360; i++)
        {
            Water1.Add(new Vector2((Mathf.Sin(i * Mathf.PI / 180.0f) * 1000), (-Mathf.Cos(i * Mathf.PI / 180.0f) * 1000)));
        }
        for (float i = 0; i < 240; i++)
        {
            Water1.Add(new Vector2((Mathf.Sin(i * Mathf.PI / 180.0f) * 1000), (Mathf.Cos(i * Mathf.PI / 180.0f) * 1000) - 2000));
        }
        recordTemplate(Water1, "6");

        // Water2 gesture
        ArrayList Water2 = new ArrayList();
        for (float i = 155; i < 190; i++)
        {
            Water2.Add(new Vector2(Mathf.Sin(i * Mathf.PI / 180.0f) * 5000, Mathf.Cos(i * Mathf.PI / 180.0f) * 1000));
        }
        for (float i = 295; i < 405; i++)
        {
            Water2.Add(new Vector2((Mathf.Sin(i * Mathf.PI / 180.0f) * 1000), (-Mathf.Cos(i * Mathf.PI / 180.0f) * 1000) - 600));
        }
        for (float i = 315; i < 405; i++)
        {
            Water2.Add(new Vector2((Mathf.Sin(i * Mathf.PI / 180.0f) * 1000) + 1400, (-Mathf.Cos(i * Mathf.PI / 180.0f) * 1000) - 600));
        }
        recordTemplate(Water2, "7");

        // Water3 gesture
        ArrayList Water3 = new ArrayList();
        for (float i = 135; i < 720; i++)
        {
            Water3.Add(new Vector2((Mathf.Sin(i * Mathf.PI / 180.0f) * 1000), (-Mathf.Cos(i * Mathf.PI / 180.0f) * 1000) - (3f * i)));
        }
        for (float i = 0; i < 240; i++)
        {
            Water3.Add(new Vector2((Mathf.Sin(i * Mathf.PI / 180.0f) * 1000), (Mathf.Cos(i * Mathf.PI / 180.0f) * 1000) - 4200));
        }
        recordTemplate(Water3, "8");

        // Earth1 gesture
        ArrayList Earth1 = new ArrayList();
        for (int i = maxPoints; i >= 0; i--)
        {
            Earth1.Add(new Vector2(0, i));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Earth1.Add(new Vector2(i, 0));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Earth1.Add(new Vector2(maxPoints, i));
        }
        recordTemplate(Earth1, "9");

        // Earth2 gesture
        ArrayList Earth2 = new ArrayList();
        for (int i = 0; i < maxPoints * 2; i++)
        {
            Earth2.Add(new Vector2(i, maxPoints * 2));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Earth2.Add(new Vector2(maxPoints * 2, (maxPoints * 2) - i));
        }
        for (int i = 0; i < maxPoints * 2; i++)
        {
            Earth2.Add(new Vector2((maxPoints * 2) - i, maxPoints));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Earth2.Add(new Vector2(0, maxPoints - i));
        }
        for (int i = 0; i < maxPoints * 2; i++)
        {
            Earth2.Add(new Vector2(i, 0));
        }
        recordTemplate(Earth2, "10");

        // Earth3 gesture
        ArrayList Earth3 = new ArrayList();
        for (int i = 0; i < maxPoints; i++)
        {
            Earth3.Add(new Vector2(0, (maxPoints * 2) - i));
        }
        for (int i = 0; i < maxPoints * 2; i++)
        {
            Earth3.Add(new Vector2(i, maxPoints));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Earth3.Add(new Vector2(maxPoints * 2, maxPoints + i));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Earth3.Add(new Vector2((maxPoints * 2) - i, maxPoints * 2));
        }
        for (int i = 0; i < maxPoints * 2; i++)
        {
            Earth3.Add(new Vector2(maxPoints, (maxPoints * 2) - i));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Earth3.Add(new Vector2(maxPoints - i, 0));
        }
        recordTemplate(Earth3, "11");

        // Dark1 gesture
        ArrayList Dark1 = new ArrayList();
        for (int i = 600; i >= 0; i--)
        {
            Dark1.Add(new Vector2((Mathf.Sin(180 * Mathf.PI / 180.0f) * 820) - i, (Mathf.Cos(180 * Mathf.PI / 180.0f) * 820) - (2 * i)));
        }
        for (float i = 180; i < 540; i++)
        {
            Dark1.Add(new Vector2(Mathf.Sin(i * Mathf.PI / 180.0f) * (1000 - i), Mathf.Cos(i * Mathf.PI / 180.0f) * (1000 - i)));
        }
        recordTemplate(Dark1, "12");

        // Dark2 gesture
        ArrayList Dark2 = new ArrayList();
        for (int i = 300; i >= 0; i--)
        {
            Dark2.Add(new Vector2((Mathf.Sin(225 * Mathf.PI / 180.0f) * 1000) + 675 + (3 * i), (Mathf.Cos(225 * Mathf.PI / 180.0f) * 1000) - (6 * i)));
        }
        for (float i = 225; i < 855; i++)
        {
            Dark2.Add(new Vector2((Mathf.Sin(i * Mathf.PI / 180.0f) * 1000) + (3f * i), Mathf.Cos(i * Mathf.PI / 180.0f) * 1000));
        }
        for (int i = 0; i < 300; i++)
        {
            Dark2.Add(new Vector2((Mathf.Sin(855 * Mathf.PI / 180.0f) * 1000) + 2565 - (3 * i), (Mathf.Cos(855 * Mathf.PI / 180.0f) * 1000) - (6 * i)));
        }
        recordTemplate(Dark2, "13");

        //Dark3 gesture
        ArrayList Dark3 = new ArrayList();
        for (float i = 430; i < 780; i++)
        {
            Dark3.Add(new Vector2((Mathf.Sin(i * Mathf.PI / 180.0f) * 1000), (-Mathf.Cos(i * Mathf.PI / 180.0f) * 1000) + (3f * i)));
        }
        for (float i = 390; i < 700; i++)
        {
            Dark3.Add(new Vector2((Mathf.Sin(i * Mathf.PI / 180.0f) * 1000) + (3f * i) - 500, (-Mathf.Cos(i * Mathf.PI / 180.0f) * 1000) + 3000));
        }
        for (float i = 300; i < 650; i++)
        {
            Dark3.Add(new Vector2((Mathf.Sin(i * Mathf.PI / 180.0f) * 1000) + 2300,
                ((-Mathf.Cos(i * Mathf.PI / 180.0f) * 1000) - (3f * i)) + 3200));
        }
        recordTemplate(Dark3, "14");

        // Light1 gesture
        ArrayList Light1 = new ArrayList();
        for (int i = 0; i < maxPoints * 2; i++)
        {
            Light1.Add(new Vector2(0, (maxPoints * 2) - i));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Light1.Add(new Vector2(0 - i, 0 + i));
        }
        for (int i = 0; i < maxPoints * 2; i++)
        {
            Light1.Add(new Vector2((0 - maxPoints) + i, maxPoints));
        }
        recordTemplate(Light1, "15");

        // Light2 gesture
        ArrayList Light2 = new ArrayList();
        for (int i = 15; i < maxPoints; i++)
        {
            Light2.Add(new Vector2(0 + i, 0 - i));
        }
        for (int i = 0; i < maxPoints * 2; i++)
        {
            Light2.Add(new Vector2(maxPoints - i, 0 - maxPoints));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Light2.Add(new Vector2((0 - maxPoints) + i, (0 - maxPoints) + i));
        }
        for (int i = 0; i < maxPoints * 2; i++)
        {
            Light2.Add(new Vector2(0, 0 - i));
        }
        recordTemplate(Light2, "16");

        // Light3 gesture
        // corner points for the pentagram
        Vector2 point1 = new Vector2(Mathf.Sin(0f * Mathf.PI / 180.0f) * 500, Mathf.Cos(0f * Mathf.PI / 180.0f) * 500);
        Vector2 point2 = new Vector2(Mathf.Sin(288f * Mathf.PI / 180.0f) * 500, Mathf.Cos(288f * Mathf.PI / 180.0f) * 500);
        Vector2 point3 = new Vector2(Mathf.Sin(216f * Mathf.PI / 180.0f) * 500, Mathf.Cos(216f * Mathf.PI / 180.0f) * 500);
        Vector2 point4 = new Vector2(Mathf.Sin(144f * Mathf.PI / 180.0f) * 500, Mathf.Cos(144f * Mathf.PI / 180.0f) * 500);
        Vector2 point5 = new Vector2(Mathf.Sin(72f * Mathf.PI / 180.0f) * 500, Mathf.Cos(72f * Mathf.PI / 180.0f) * 500);

        ArrayList Light3 = new ArrayList();
        for (int i = 0; i < maxPoints; i++)
        {
            Light3.Add(new Vector2(point1.x + ((point3.x - point1.x) / maxPoints * i), point1.y + ((point3.y - point1.y) / maxPoints * i)));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Light3.Add(new Vector2(point3.x + ((point5.x - point3.x) / maxPoints * i), point3.y + ((point5.y - point3.y) / maxPoints * i)));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Light3.Add(new Vector2(point5.x + ((point2.x - point5.x) / maxPoints * i), point5.y + ((point2.y - point5.y) / maxPoints * i)));
        }
        for (int i = 0; i < maxPoints; i++)
        {
            Light3.Add(new Vector2(point2.x + ((point4.x - point2.x) / maxPoints * i), point2.y + ((point4.y - point2.y) / maxPoints * i)));
        }
        for (int i = 0; i < maxPoints - 15; i++)
        {
            Light3.Add(new Vector2(point4.x + ((point1.x - point4.x) / maxPoints * i), point4.y + ((point1.y - point4.y) / maxPoints * i)));
        }
        recordTemplate(Light3, "17");
    }

    public void startRecognizer(ArrayList pointArray)
    {
        // main recognizer function
        pointArray = optimizeGesture(pointArray);
        pointArray = ScaleGesture(pointArray);
        pointArray = TranslateGestureToOrigin(pointArray);

        ////Fill string with gesture data and write to file for testing
        //string text = "test";
        //for (int i = 0; i < pointArray.Count; i++)
        //{
        //    Vector2 v = (Vector2)pointArray[i];
        //    text += ";" + v.x + "," + v.y;
        //}

        ////Write gesture to file        
        //FileStream fs = new FileStream(Application.dataPath + "/Resources/DrawnGestures" + maxPoints + ".txt", FileMode.Append, FileAccess.Write);
        //using (StreamWriter sw = new StreamWriter(fs))
        //{
        //    sw.WriteLine(text);
        //}

        gestureMatch(pointArray);
    }

    public void recordTemplate(ArrayList pointArray, string gestureID)
    {
        if (gestureID != "" && gestureID != null)
        {
            // record function
            pointArray = optimizeGesture(pointArray);
            pointArray = ScaleGesture(pointArray);
            pointArray = TranslateGestureToOrigin(pointArray);

            //Fill string with gesture data
            string text = string.Join("", gestureID.Split(';'));
            for (int i = 0; i < pointArray.Count; i++)
            {
                Vector2 v = (Vector2)pointArray[i];
                text += ";" + v.x + "," + v.y;
            }

            //Write gesture to file        
            FileStream fs = new FileStream(Application.dataPath + "/Resources/Gestures" + maxPoints + ".txt", FileMode.Append, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(text);
            }

            //Add gesture to array
            TemplateNames.Add(string.Join("", gestureID.Split(';')));
            Templates.Add(pointArray);
        }
    }

  ArrayList optimizeGesture(ArrayList pointArray)
    {
        // take all the points in the gesture and finds the correct points compared with distance and the maximun value of points

        // calc the interval relative the length of the gesture drawn by the user
        float interval = calcTotalGestureLength(pointArray) / (maxPoints - 1);

        // use the same starting point in the new array from the old one. 
        ArrayList optimizedPoints = new ArrayList();
        optimizedPoints.Add(pointArray[0]);

        float tempDistance = 0.0f;

        // run through the gesture array. Start at i = 1 because we compare point two with point one)
        for (int i = 1; i < pointArray.Count; ++i)
        {
            float currentDistanceBetween2Points = calcDistance((Vector2)pointArray[i - 1], (Vector2)pointArray[i]);

            if ((tempDistance + currentDistanceBetween2Points) >= interval)
            {
                Vector2 v1 = (Vector2)pointArray[i - 1];
                Vector2 v = (Vector2)pointArray[i];

                // the calc is: old pixel + the differens of old and new pixel multiply  
                float newX = v1.x + ((interval - tempDistance) / currentDistanceBetween2Points) * (v.x - v1.x);
                float newY = v1.y + ((interval - tempDistance) / currentDistanceBetween2Points) * (v.y - v1.y);

                // create new point
                Vector2 newPoint = new Vector2(newX, newY);

                // set new point into array
                optimizedPoints.Add(newPoint);

                ArrayList temp = pointArray.GetRange(i, pointArray.Count - i - 1);
                Vector2 last = (Vector2)pointArray[pointArray.Count - 1];
                pointArray.SetRange(i + 1, temp);
                pointArray.Add(last);
                //pointArray.InsertRange(i + 1, temp);
                pointArray.Insert(i, newPoint);

                tempDistance = 0.0f;
            }
            else
            {
                // the point was too close to the last point compared with the interval,. Therefore the distance will be stored for the next point to be compared.
                tempDistance += currentDistanceBetween2Points;
            }
        }

        // Rounding-errors might happens. Just to check if all the points are in the new array
        if (optimizedPoints.Count == maxPoints - 1)
        {
            Vector2 v = (Vector2)pointArray[pointArray.Count - 1];
            optimizedPoints.Add(new Vector2(v.x, v.y));
        }

        return optimizedPoints;
    }

 ArrayList ScaleGesture(ArrayList pointArray)
    {
        // equal min and max to the opposite infinity, such that every gesture size can fit the bounding box.
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;

        // loop through array. Find the minimum and maximun values of x and y to be able to create the box
        foreach (Vector2 v in pointArray)
        {
            if (v.x < minX) minX = v.x;
            if (v.x > maxX) maxX = v.x;
            if (v.y < minY) minY = v.y;
            if (v.y > maxY) maxY = v.y;
        }

        // create a rectangle surronding the gesture as a bounding box.
        Rect BoundingBox = new Rect(minX, minY, maxX - minX, maxY - minY);
        ArrayList newArray = new ArrayList();

        foreach (Vector2 v in pointArray)
        {
            float newX = v.x * (sizeOfScaleRect / BoundingBox.width);
            float newY = v.y * (sizeOfScaleRect / BoundingBox.height);
            newArray.Add(new Vector2(newX, newY));
        }

        return newArray;
    }


 ArrayList TranslateGestureToOrigin(ArrayList pointArray)
    {
        Vector2 origin = new Vector2(0, 0);
        Vector3 center = calcCenterOfGesture(pointArray);
        ArrayList newArray = new ArrayList();

        foreach (Vector2 v in pointArray)
        {
            float newX = v.x + origin.x - center.x;
            float newY = v.y + origin.y - center.y;
            newArray.Add(new Vector2(newX, newY));
        }

        return newArray;
    }


    // --------------------------------  		     GESTURE OPTIMIZING DONE   		----------------------------------------------------------------
    // -------------------------------- 		START OF THE MATCHING PROCESS	----------------------------------------------------------------



 void gestureMatch(ArrayList pointArray)
    {
        float tempDistance = Mathf.Infinity;
        int count = 0;

        for (int i = 0; i < Templates.Count; ++i)
        {
            float distance = calcGestureTemplateDistance(pointArray, (ArrayList)Templates[i]);

            if (distance < tempDistance)
            {
                tempDistance = distance;
                count = i;
            }
        }

        float HalfDiagonal = 0.5f * Mathf.Sqrt(Mathf.Pow(sizeOfScaleRect, 2) + Mathf.Pow(sizeOfScaleRect, 2));
        float score = 1.0f - (tempDistance / HalfDiagonal);

        // print the result

        if (score < scoreThreshold)
        {
            Debug.Log("NO MATCH " + score);
            sg.FailSpell(score);
        }
        else {
            Debug.Log("RESULT: " + TemplateNames[count] + " SCORE: " + score);

            if (sg.RetrieveSpell("" + TemplateNames[count], score))
            {
                ps.Emit(1);
                drawCam.GetComponent<AudioSource>().pitch = Random.Range(0.90f, 1.10f);
                drawCam.GetComponent<AudioSource>().Play();
            }
        }
    }


    // --------------------------------  		   GESTURE RECOGNIZER DONE   		----------------------------------------------------------------
    // -------------------------------- 		START OF THE HELP FUNCTIONS		----------------------------------------------------------------


 Vector2 calcCenterOfGesture(ArrayList pointArray)
    {
        // finds the center of the drawn gesture

        float averageX = 0.0f;
        float averageY = 0.0f;

        foreach (Vector2 v in pointArray)
        {
            averageX += v.x;
            averageY += v.y;
        }

        averageX = averageX / pointArray.Count;
        averageY = averageY / pointArray.Count;

        return new Vector2(averageX, averageY);
    }

 float calcDistance(Vector2 point1, Vector2 point2)
    {
        // distance between two vector points.
        float dx = point2.x - point1.x;
        float dy = point2.y - point1.y;

        return Mathf.Sqrt(dx * dx + dy * dy) * factor;
    }

 float calcTotalGestureLength(ArrayList pointArray)
    {
        // total length of gesture path
        float length = 0.0f;
        for (int i = 1; i < pointArray.Count; ++i)
        {
            length += calcDistance((Vector2)pointArray[i - 1], (Vector2)pointArray[i]);
        }

        return length;
    }

 float calcGestureTemplateDistance(ArrayList userGesturePoints, ArrayList templatePoints)
    {
        // calc the distance between gesture path from user and the template gesture
        float distance = 0.0f;

        // assumes newRotatedPoints.length == templatePoints.length
        for (int i = 0; i < userGesturePoints.Count; ++i)
        {
            distance += calcDistance((Vector2)userGesturePoints[i], (Vector2)templatePoints[i]);
        }

        return distance / userGesturePoints.Count;
    }
}
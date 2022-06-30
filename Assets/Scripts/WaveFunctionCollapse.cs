using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// https://www.youtube.com/watch?v=2SuvO4Gi7uY&t=96s
[Serializable]
public class Module
{
    public string objectName;
    public int objectRotation;
    public string[] socketList;
    public string constrainTo;
    public string constrainFrom;
    public int weight;
    public string[] validNeighbours;

    public Module(string objectName, int rotation, string pX, string nX, string pY, string nY, string pZ, string nZ, string constrainTo, string constrainFrom, int weight)
    {
        this.objectName = objectName;
        objectRotation = rotation;

        // Initialise the socketList
        socketList = new string[6];
        socketList[0] = pX;
        socketList[1] = nX;
        socketList[2] = pY;
        socketList[3] = nY;
        socketList[4] = pZ;
        socketList[5] = nZ;

        this.constrainTo = constrainTo;
        this.constrainFrom = constrainFrom;
        this.weight = weight;
        this.validNeighbours = new string[4] { "0s", "v0_0", "1s", "-1" };

    }

}

public class WaveFunctionCollapse
{
    private string jsonModulePath = "..\\Unity Tower Defence Game\\Assets\\Scripts\\JSON\\Module_List.json";
    private List<Module> moduleList;
    private Dictionary<Vector3, List<Module>> _waveFunction = new();
    public Dictionary<Vector3, List<Module>> waveFunction
    {
        get { return _waveFunction; }
    }
    private Vector3 size;
    private float numRemaining;
    private bool isCollapsed = false;

    public void Initialise(Vector3 mapSize)
    {

        string jsonString = System.IO.File.ReadAllText(jsonModulePath);
        moduleList = ConvertJSONToModuleList(jsonString);
        size = mapSize;
        numRemaining = mapSize.x * mapSize.y * mapSize.z;

        // Loop through all points on the map and assign all modules to those positions.
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    _waveFunction.Add(new Vector3(x, y, z), moduleList);
                }
            }
        }

        while (!isCollapsed) {
            Iterate();
        }

    }

    private void Iterate()
    {
        List<Vector3> coords = GetMinEntropyCoords();
        CollapsAt(coords);
        numRemaining -= 1;
        if (numRemaining < 1)
        {
            isCollapsed = true;
            return;
        }
        Propagate(coords);
    }
    // Removes all other module choices at given coords.
    // Chooses a coord at random.
    private void CollapsAt(List<Vector3> coords)
    {
        Vector3 chosenCoord;
        System.Random rnd = new System.Random();
        int num;

        if (coords.Count == 1)
        {
            chosenCoord = coords[0];
        }
        else
        { 
            num = rnd.Next(coords.Count);
            chosenCoord = coords[num];
        }

        _waveFunction.TryGetValue(chosenCoord, out List<Module> moduleList);
        num = rnd.Next(moduleList.Count);
        Module chosenModule = moduleList[num];
        moduleList.Clear();
        moduleList.Add(chosenModule);
        Debug.Log("ChosenTile: " + chosenModule.objectName);
    }
    
    private void Propagate(List<Vector3> coords)
    {
        List<Vector3> stack = coords;

        while (stack.Count > 0)
        {
            // Pop the last element of the stack
            Vector3 coord = stack[0];
            stack.RemoveAt(0);

            // Loop through each of the sides
            for (int i = 0; i < 6; i++)
            {
                List<string> validSockits = new List<string>();
                _waveFunction.TryGetValue(coord, out List<Module> moduleList);
                foreach (Module module in moduleList)
                {
                    validSockits.Add(module.socketList[i]);
                }

                Vector3 otherCoord = coord + GetDirection(i);

                // Compare coords with other coords
                if (_waveFunction.TryGetValue(otherCoord, out List<Module> otherModuleList))
                {
                    List<Module> newModuleList = new List<Module>();// Create a new list for valid modules
                    foreach (Module otherModule in otherModuleList)// Loop through other coord list
                    {
                        if (validSockits.Contains(otherModule.socketList[i])) {
                            newModuleList.Add(otherModule);// Add to new list if valid
                        }
                    }

                    if (otherModuleList.Count > newModuleList.Count)// If there are less modules in new module list
                    {
                        if(!stack.Contains(otherCoord))// Check if coord already exists on stack
                        {
                            stack.Add(otherCoord);// Add if it doesn't exist add to stack
                        }
                        _waveFunction[otherCoord] = newModuleList;// Add new list of coords to dictionary
                    }
                }

            }
        }
    }

    private Vector3 GetDirection(int d)
    {
        switch (d)
        {
            case 0:
                return new Vector3(1, 0, 0); //pX
            case 1:
                return new Vector3(-1, 0, 0); //nX
            case 2:
                return new Vector3(0, 1, 0); //pY
            case 3:
                return new Vector3(0, -1, 0); //nY
            case 4:
                return new Vector3(0, 0, 1); //pZ
            case 5:
                return new Vector3(0, 0, -1); //nZ
            default:
                Debug.LogError("Error, direction not in any 3D space!.");
                return new Vector3(0,0,0);
        }
    }
    // Get a list of all coords with the smallest entropy
    private List<Vector3> GetMinEntropyCoords()
    {
        List<Vector3> minEntropyList = new List<Vector3>();
        int minEntropy = moduleList.Count;
        foreach(KeyValuePair<Vector3,List<Module>> entry in _waveFunction)
        {
            if (entry.Value.Count > minEntropy) { }
            else if (entry.Value.Count == minEntropy)
            {
                minEntropyList.Add(entry.Key);
            }
            else
            {
                minEntropyList.Clear();
                minEntropyList.Add(entry.Key);
                minEntropy = entry.Value.Count;
            }
        }

        return minEntropyList;
    }

    // Takes in a json string and creates a module list from it.
    private List<Module> ConvertJSONToModuleList(string jsonString)
    {
        List<Module> moduleList = new List<Module>();

        List<string> moduleStringList = jsonString.Split(new string[] { "}", "{" }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();

        moduleStringList.RemoveAll(s => s.Length < 15);

        foreach (string moduleString in moduleStringList)
        {
            moduleList.Add(CreateModuleFromString(moduleString));
        }

        return moduleList;
    }

    // Creates Module from a string.
    private Module CreateModuleFromString(string moduleString)
    {
        List<string> items = new List<string>();
        List<string> itemList = moduleString.Split(",").ToList<string>();

        foreach (string item in itemList)
        {
            string[] keyItem = item.Split(":");
            if (keyItem.Length > 1)
            {
                string newItem = keyItem[1].Replace("\"", "");
                newItem = newItem.Replace(" ", "");
                items.Add(newItem);
            }
        }

        Module m = new(items[0], int.Parse(items[1]), items[2], items[3], items[4], items[5],
                       items[6], items[7], items[8], items[9], int.Parse(items[10]));

        return m;
    }
}

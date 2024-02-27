namespace LevelGenerator2D
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : Editor
    {
        private const string SpawnChanceLabel = "Spawn Chance";
        private const string SpawnChanceTooltip = "This is the chance that a GameObject will be successfully spawned when attempted.  0 means no GameObject will every spawn, and 1 means every GameObject will always spawn.";
        private const string DefaultLabel = "Add GameObject";
        private const string GameObjectTooltip = "GameObject which can be spawned at a given rate.";
        private const string SpawnOnStartLabel = "Spawn on Start";
        private const string SpawnOnStartTooltip = "Whether or not to spawn the GameObject on start, opposed to waiting for Spawn() to be called manually";
        private const string SpawnCountLabelPrefix = "Spawn Count";
        private const string SpawnCountTooltip = "How many GameObjects to spawn";
        private const float MinSpawnChance = 0.0f;
        private const float MaxSpawnChance = 1.0f;
        private const int MinSpawnCount = 1;
        private const int MaxSpawnCount = 25;
        private const string RemoveSpawnableButtonName = "X";
        private const float RemoveSpawnableButtonWidth = 25.0f;

        public override void OnInspectorGUI()
        {
            var spawner = (Spawner)target;
            var spawnChanceGuiContent = new GUIContent(SpawnChanceLabel, SpawnChanceTooltip);
            float spawnChance = EditorGUILayout.Slider(spawnChanceGuiContent, spawner.GetSpawnChance(), MinSpawnChance, MaxSpawnChance);
            spawner.SetSpawnChance(spawnChance);
            var onStartGuiContent = new GUIContent(SpawnOnStartLabel, SpawnChanceTooltip);
            bool spawnOnStart = EditorGUILayout.Toggle(onStartGuiContent, spawner.GetSpawnOnStart());
            spawner.SetSpawnOnStart(spawnOnStart);
            float minSpawns = spawner.GetMinSpawns();
            float maxSpawns = spawner.GetMaxSpawns();
            string spawnCountLabel = SpawnCountLabelPrefix + " [" + minSpawns + "," + maxSpawns + "]";
            var spawnCountGuiContent = new GUIContent(spawnCountLabel, SpawnCountTooltip);
            EditorGUILayout.MinMaxSlider(spawnCountGuiContent, ref minSpawns, ref maxSpawns, MinSpawnCount, MaxSpawnCount);
            spawner.SetMinSpawns((int)minSpawns);
            spawner.SetMaxSpawns((int)maxSpawns);
            var spawnables = spawner.GetSpawnables();
            var weights = spawner.GetWeights();
            for (int i = 0; i < spawnables.Count && i < weights.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                var existingGuiContent = new GUIContent(spawnables[i].name, GameObjectTooltip);
                weights[i] = EditorGUILayout.FloatField(existingGuiContent, weights[i]);
                var updatedSpawnable = (GameObject)EditorGUILayout.ObjectField(spawnables[i], typeof(GameObject), false);
                if (GUILayout.Button(RemoveSpawnableButtonName, GUILayout.Width(RemoveSpawnableButtonWidth)) || !updatedSpawnable)
                {
                    spawnables.RemoveAt(i);
                    weights.RemoveAt(i);
                }
                else
                {
                    spawnables[i] = updatedSpawnable;
                }
                EditorGUILayout.EndHorizontal();
            }
            var nonExistingGuiContent = new GUIContent(DefaultLabel, GameObjectTooltip);
            var newSpawnable = (GameObject)EditorGUILayout.ObjectField(nonExistingGuiContent, null, typeof(GameObject), false);
            if (newSpawnable)
            {
                spawner.AddSpawnable(newSpawnable);
            }
            spawner.SetMaxSpawnableSize();
        }
    }
}
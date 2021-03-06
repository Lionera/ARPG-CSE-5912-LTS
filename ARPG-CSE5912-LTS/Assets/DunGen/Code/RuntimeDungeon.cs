using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DunGen.Adapters;
using UnityEngine.AI;

namespace DunGen
{
    [AddComponentMenu("DunGen/Runtime Dungeon")]

	public class RuntimeDungeon : MonoBehaviour
	{
        public DungeonGenerator Generator = new DungeonGenerator();
        public bool GenerateOnStart = true;
		public GameObject Root;
		public SaveDungeon savedDunData;

        protected virtual void Awake()
        {
			if (GenerateOnStart)
			{
				SetStart();
				Generate();
				//BakeNavMesh();
				//AddEnemies();
				SetWayport();
				Debug.Log("Dungeon Generated!");
			}
		}

		public void Generate()
		{
			if(Root != null)
				Generator.Root = Root;

			if (savedDunData != null)
				Generator.savedDungeon = savedDunData;

			if (!Generator.IsGenerating)
				Generator.Generate();
		}

		void BakeNavMesh()
        {
			foreach (Transform child in Root.transform)
			{
				if (child.CompareTag("DungeonPart"))
				{
					child.GetComponent<NavMeshSurface>().BuildNavMesh();
				}
			}
		}

		void AddEnemies()
        {
			//enable navmesh agents (for spawning conflict avoidance)
			var agents = UnityEngine.Object.FindObjectsOfType<NavMeshAgent>();
			foreach (NavMeshAgent agent in agents)
			{
				agent.enabled = true;
			}
		}

		void SetWayport()
        {
			Debug.Log("Set waypoint");
			var waypointRoom = GameObject.FindWithTag("WaypointRoom");
			if (waypointRoom != null)
            {
				var loc = waypointRoom.transform.position + Vector3.up;
				savedDunData.SetWaypointLocation(loc);
			}
		}

		void SetStart()
		{
			if (Root != null)
			{
				var loc = Root.transform.position + Vector3.up;
				savedDunData.SetDungeonStartLocation(loc);
			}
		}

	}
}

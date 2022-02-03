﻿using System.Collections;
using UnityEngine;

namespace LootLabels {
    /// <summary>
    /// Example class for container objects
    /// </summary>
    [RequireComponent(typeof(CreateLabel))]
    [RequireComponent(typeof(ObjectHighlight))]
    public class Container : InteractableObject {
        public string objectName;    //the name of the object, this name is shown in the labels
        public LootSource lootSource;   //the rarity of the source to determine loot drops
        
        bool chestOpened = false;   //Toggle to check if the chest has been opened yet
        
        // Use this for initialization
        void Start() {
            SpawnLabel();
        }

        /// <summary>
        /// Creates the label!!
        /// Called in animator or in start when there is no animator
        /// </summary>
        public override void SpawnLabel() {
            GetComponent<EventHandler>().ClearDelegates();
            GetComponent<EventHandler>().SubscribeMouseEvents(MouseDownFunction, MouseEnterFunction, MouseExitFunction);

            GetComponent<CreateLabel>().SpawnLabelByColor(objectName, "LootLabels/Icons/UI_Icon_Bag1");

            StartCoroutine(GetComponent<EventHandler>().VisibilityCoroutine());
        }

        void OpenChest() {
            if (!chestOpened) {
                chestOpened = true;
                GetComponent<ObjectHighlight>().StopHighlightObject();

                if (GetComponent<AudioSource>()) {
                    GetComponent<AudioSource>().Play();
                }

                if (GetComponent<Animation>()) {
                    GetComponent<Animation>().Play("open");
                }

                StartCoroutine(DropLootCoroutine());
            }
        }

        //Wait half a second for the chest open animation to finish and then start dropping legendaries
        IEnumerator DropLootCoroutine() {
            yield return new WaitForSeconds(.5f);
            LootManager.singleton.DropLoot(lootSource, transform);
        }

        #region mouse functions
        public override void MouseDownFunction() {
            if (chestOpened) {
                return;
            }

            OpenChest();
        }

        public override void MouseEnterFunction() {
            if (chestOpened) {
                return;
            }

            GetComponent<ObjectHighlight>().HighlightObject();
        }

        public override void MouseExitFunction() {
            if (chestOpened) {
                return;
            }

            GetComponent<ObjectHighlight>().StopHighlightObject();
        }
        #endregion
    }
}

using Assets.Behaviours.Cutscene;
using Assets.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Behaviours.LevelSpecific
{
    class EndDoorBehaviour : MonoBehaviour, IInteractable
    {
        public AudioClip _creditsMusic;
        public TextAsset _conversation;

        private readonly Lazy<GameObject> _hero;
        private readonly Lazy<CutsceneControllerBehaviour> _cutsceneController;
        private readonly Lazy<GameObject> _credits;

        public EndDoorBehaviour()
        {
            _hero = new Lazy<GameObject>(() => SceneManager.GetActiveScene().GetRootGameObjects().First(x => x.name == "HeroPlayer"));
            _credits = new Lazy<GameObject>(() => SceneManager.GetActiveScene().GetRootGameObjects().First(x => x.name == "Credits"));
            _cutsceneController = new Lazy<CutsceneControllerBehaviour>(() => FindObjectOfType<CutsceneControllerBehaviour>());
        }

        public bool CanInteractWith(PlayerControllerBehaviour player) => Mathf.Abs(player.transform.position.x - transform.position.x) <= 0.5 * player.GetComponent<Collider2D>().bounds.size.x
                && Mathf.Abs(player.transform.position.y - transform.position.y) <= 0.5 * player.GetComponent<Collider2D>().bounds.size.y;

        public void InteractWith(PlayerControllerBehaviour player)
        {
            StartCoroutine(CutsceneCoroutine(player));
        }

        private IEnumerator CutsceneCoroutine(PlayerControllerBehaviour player)
        {
            FindObjectOfType<LevelControllerBehaviour>().restartOnNoPlayers = false;
            Destroy(player.gameObject);
            yield return new WaitForSeconds(2);

            _hero.Value.SetActive(true);
            _hero.Value.GetComponent<PlayerControllerBehaviour>().enabled = false;

            _cutsceneController.Value.RegisterSpeaker("Doomguy", _hero.Value.GetComponent<TalkBehaviour>());
            var physicsObject = _hero.Value.GetComponent<PhysicsObject>();
            var controller = _hero.Value.GetComponent<PlayerControllerBehaviour>();
            var marker = SceneManager.GetActiveScene().GetRootGameObjects().First(x => x.name == "Marker");
            var marker2 = SceneManager.GetActiveScene().GetRootGameObjects().First(x => x.name == "Marker2");

            physicsObject.PositionOnGround();
            yield return StartCoroutine(_cutsceneController.Value.WalkToX(physicsObject, marker.transform.position.x, controller.runSpeed * 0.5f));
            yield return StartCoroutine(_cutsceneController.Value.PlayConversationCoroutine(JsonUtility.FromJson<Conversation>(_conversation.text)));
            yield return new WaitForSeconds(1f);

            FindObjectOfType<MusicController>().SetMusic(_creditsMusic, true);
            _credits.Value.SetActive(true);
            //physicsObject.YVelocity = controller.jumpVelocity;
            //StartCoroutine(_cutsceneController.Value.WalkToX(physicsObject, marker2.transform.position.x, controller.runSpeed));
            
        }
    }
}

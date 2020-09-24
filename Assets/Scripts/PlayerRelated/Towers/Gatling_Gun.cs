using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


namespace GameDevHQ.FileBase.Gatling_Gun
{
    [RequireComponent(typeof(AudioSource))] //Require Audio Source component
    public class Gatling_Gun : Tower
    {
        [SerializeField]
        private Transform[] _gunBarrel; //Reference to hold the gun barrel
        public GameObject[] Muzzle_Flash; //reference to the muzzle flash effect to play when firing
        public ParticleSystem[] bulletCasings; //reference to the bullet casing effect to play when firing
        public AudioClip fireSound; //Reference to the audio clip

        private AudioSource _audioSource; //reference to the audio source component
        private bool _startWeaponNoise = true;

        // Use this for initialization
        void Start()
        {

             //assigning the transform of the gun barrel to the variable
            for (int i = 0; i < Muzzle_Flash.Length; i++)
            {
                Muzzle_Flash[i].SetActive(false);
            }
            //setting the initial state of the muzzle flash effect to off
            _audioSource = GetComponent<AudioSource>(); //ssign the Audio Source to the reference variable
            _audioSource.playOnAwake = false; //disabling play on awake
            _audioSource.loop = true; //making sure our sound effect loops
            _audioSource.clip = fireSound; //assign the clip to play
        }

        // Update is called once per frame
        void FireGatlingGun()
        {

            RotateBarrel(); //Call the rotation function responsible for rotating our gun barrel
            for (int i = 0; i < Muzzle_Flash.Length; i++)
            {
                Muzzle_Flash[i].SetActive(true);//enable muzzle effect particle effect
            }
            for (int i = 0; i < bulletCasings.Length; i++)
            {
                bulletCasings[i].Emit(1); //Emit the bullet casing particle effect  
            }




            if (_startWeaponNoise == true) //checking if we need to start the gun sound
            {
                _audioSource.Play(); //play audio clip attached to audio source
                _startWeaponNoise = false; //set the start weapon noise value to false to prevent calling it again
            }


        }
        public override void StopFiring()
        {
            base.StopFiring();
            for (int i = 0; i < Muzzle_Flash.Length; i++)
            {
                Muzzle_Flash[i].SetActive(false); //turn off muzzle flash particle effect
            }
            _audioSource.Stop(); //stop the sound effect from playing
            _startWeaponNoise = true; //set the start weapon noise value to true
        }

        // Method to rotate gun barrel 
        void RotateBarrel()
        {
            for (int i = 0; i < _gunBarrel.Length; i++)
            {
                //rotate the gun barrel along the "forward" (z) axis at 500 meters per second
                _gunBarrel[i].transform.Rotate(Vector3.forward * Time.deltaTime * -500.0f); 
            }
            
        }

        public override void TargetEnemy()
        {
            base.TargetEnemy();
            FireGatlingGun();
        }
    }

}

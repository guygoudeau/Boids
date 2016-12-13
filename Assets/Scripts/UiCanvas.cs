// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UICanvas.cs" company="Guy Goudeau">
//   Property of Guy Goudeau, do not steal.
// </copyright>
// <summary>
//   Defines the UiCanvas type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// The UI canvas class.
    /// </summary>
    public class UiCanvas : MonoBehaviour
    {
        /// <summary>
        /// The cohesion slider.
        /// </summary>
        [SerializeField]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Slider cohesion;

        /// <summary>
        /// The dispersion slider.
        /// </summary>
        [SerializeField]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Slider dispersion;

        /// <summary>
        /// The alignment slider.
        /// </summary>
        [SerializeField]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Slider alignment;

        /// <summary>
        /// The BOIDs instance.
        /// </summary>
        [SerializeField]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Boids boids;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiCanvas"/> class.
        /// </summary>
        /// <param name="cohesion">
        /// The cohesion slider.
        /// </param>
        /// <param name="dispersion">
        /// The dispersion slider.
        /// </param>
        /// <param name="alignment">
        /// The alignment slider.
        /// </param>
        /// <param name="boids">
        /// The BOID instance.
        /// </param>
        public UiCanvas(Slider cohesion, Slider dispersion, Slider alignment,  Boids boids)
        {
            this.cohesion = cohesion;
            this.dispersion = dispersion;
            this.alignment = alignment;
            this.boids = boids;
        }

        /// <summary>
        /// Reloads the scene.
        /// </summary>
        /// <param name="sceneName">
        /// The name of the scene.
        /// </param>
        public void Reload(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Initializes starting values.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            this.cohesion.value = this.boids.CohesionMod;
            this.dispersion.value = this.boids.DispersionMod;
            this.alignment.value = this.boids.AlignmentMod;
        }

        /// <summary>
        /// Updates scene every frame.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            this.boids.CohesionMod = this.cohesion.value;
            this.boids.DispersionMod = this.dispersion.value;
            this.boids.AlignmentMod = this.alignment.value;
        }
    }
}
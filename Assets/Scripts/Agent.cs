// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Agent.cs" company="Guy Goudeau">
//   Property of Guy Goudeau, do not steal.
// </copyright>
// <summary>
//   Defines the Agent type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using UnityEngine;

    /// <summary>
    /// The agent.
    /// </summary>
    public class Agent : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the velocity.
        /// </summary>
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// Updates the scene every frame.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            this.transform.position = this.Position;
        }
    }
}

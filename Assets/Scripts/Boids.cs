// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Boids.cs" company="Guy Goudeau">
//   Property of Guy Goudeau, do not steal.
// </copyright>
// <summary>
//   Defines the Boids type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The BOIDS class.
    /// </summary>
    public class Boids : MonoBehaviour
    {
        /// <summary>
        /// The number of BOIDs.
        /// </summary>
        private const uint NumberOfBoids = 25;

        /// <summary>
        /// The BOID list.
        /// </summary>
        [SerializeField]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private List<Agent> boidsList;

        /// <summary>
        /// The BOID prefab.
        /// </summary>
        [SerializeField]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private GameObject boidPrefab;

        /// <summary>
        /// The cohesion mod.
        /// </summary>
        [Range(0.0f, 1.0f)]
        private float cohesionMod;

        /// <summary>
        /// The dispersion mod.
        /// </summary>
        [Range(0.0f, 1.0f)]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private float dispersionMod;

        /// <summary>
        /// The alignment mod.
        /// </summary>
        [Range(0.0f, 1.0f)]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private float alignmentMod;

        /// <summary>
        /// Initializes a new instance of the <see cref="Boids"/> class.
        /// </summary>
        /// <param name="boidPrefab">
        /// The BOID prefab.
        /// </param>
        /// <param name="cohesionMod">
        /// The cohesion mod.
        /// </param>
        /// <param name="dispersionMod">
        /// The dispersion mod.
        /// </param>
        /// <param name="alignmentMod">
        /// The alignment mod.
        /// </param>
        public Boids(GameObject boidPrefab, float cohesionMod, float dispersionMod, float alignmentMod)
        {
            this.boidPrefab = boidPrefab;
            this.CohesionMod = cohesionMod;
            this.dispersionMod = dispersionMod;
            this.alignmentMod = alignmentMod;
            this.boidsList = new List<Agent>();
        }

        /// <summary>
        /// Gets or sets the cohesion mod.
        /// </summary>
        public float CohesionMod
        {
            get
            {
                return this.cohesionMod;
            }

            set
            {
                this.cohesionMod = value;
            }
        }

        /// <summary>
        /// Gets or sets the dispersion mod.
        /// </summary>
        public float DispersionMod
        {
            get
            {
                return this.dispersionMod;
            }

            set
            {
                this.dispersionMod = value;
            }
        }

        /// <summary>
        /// Gets or sets the alignment mod.
        /// </summary>
        public float AlignmentMod
        {
            get
            {
                return this.alignmentMod;
            }

            set
            {
                this.alignmentMod = value;
            }
        }

        /// <summary>
        /// The rule to tend towards a place.
        /// </summary>
        /// <param name="thisBoid">
        /// This BOID.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private static Vector3 TendTowardsPlace(Agent thisBoid)
        {
            var place = Vector3.zero;

            return (place - thisBoid.Position) / 100;
        }

        /// <summary>
        /// The rule to limit velocity.
        /// </summary>
        /// <param name="thisBoid">
        /// This BOID.
        /// </param>
        private static void LimitVelocity(Agent thisBoid)
        {
            const int Limiter = 15;

            if (thisBoid.Velocity.magnitude > Limiter)
            {
                thisBoid.Velocity = (thisBoid.Velocity / thisBoid.Velocity.magnitude).normalized * Limiter; 
            }
        }

        /// <summary>
        /// Takes care of actions that need to be done at program start.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            this.InitialisePosition();
        }

        /// <summary>
        /// Updates the scene every frame.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            this.MoveBoids();
        }

        /// <summary>
        /// Initializes BOIDs starting positions.
        /// </summary>
        private void InitialisePosition()
        {
            for (var i = 0; i < NumberOfBoids; i++)
            {
                var boids = Instantiate(this.boidPrefab);
                boids.GetComponent<Agent>().Position = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
                this.boidsList.Add(boids.GetComponent<Agent>());
            }
        }

        /// <summary>
        /// Updates the BOID's positions.
        /// </summary>
        private void MoveBoids()
        {
            foreach (var allBoids in this.boidsList)
            {
                var v1 = this.Cohesion(allBoids);
                var v2 = this.Dispersion(allBoids);
                var v3 = this.Alignment(allBoids);
                var v4 = TendTowardsPlace(allBoids);

                allBoids.Velocity += v1 + v2 + v3 + v4;
                LimitVelocity(allBoids);
                allBoids.Position += allBoids.Velocity.normalized;
            }
        }

        /// <summary>
        /// The cohesion rule.
        /// </summary>
        /// <param name="thisBoid">
        /// This BOID.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private Vector3 Cohesion(Agent thisBoid)
        {
            var perceivedCenter = Vector3.zero;
            foreach (var otherBoid in this.boidsList)
            {
                if (otherBoid != thisBoid)
                {
                    perceivedCenter += otherBoid.Position;
                }
            }

            perceivedCenter = perceivedCenter / (NumberOfBoids - 1);

            return (perceivedCenter - thisBoid.Position).normalized * this.CohesionMod;
        }

        /// <summary>
        /// The dispersion rule.
        /// </summary>
        /// <param name="thisBoid">
        /// This BOID.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private Vector3 Dispersion(Agent thisBoid)
        {
            var displacement = Vector3.zero;
            foreach (var otherBoid in this.boidsList)
            {
                if (otherBoid == thisBoid)
                {
                    continue;
                }

                if ((otherBoid.Position - thisBoid.Position).magnitude < 100)
                {
                    displacement -= otherBoid.Position - thisBoid.Position;
                }
            }

            return displacement.normalized * this.dispersionMod;
        }

        /// <summary>
        /// The alignment rule.
        /// </summary>
        /// <param name="thisBoid">
        /// This BOID.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private Vector3 Alignment(Agent thisBoid)
        {
            var percievedVelocity = Vector3.zero;
            foreach (var otherBoid in this.boidsList)
            {
                if (otherBoid != thisBoid)
                {
                    percievedVelocity += otherBoid.Velocity;
                }
            }

            percievedVelocity = percievedVelocity / (NumberOfBoids - 1);

            return ((percievedVelocity - thisBoid.Velocity).normalized / 8) * this.alignmentMod;
        }

/*
        /// <summary>
        /// The rule to bind position.
        /// </summary>
        /// <param name="thisBoid">
        /// This BOID.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private Vector3 BindPosition(Agent thisBoid)
        {
            float xMin = -5;
            float xMax = 5;
            float yMin = -5;
            float yMax = 5;
            float zMin = -5;
            float zMax = 5;
            Vector3 pushBack = Vector3.zero;

            if (thisBoid.Position.x < xMin)
            {
                pushBack.x = 3;
            }
            else if (thisBoid.Position.x > xMax)
            {
                pushBack.x = -3;
            }
            if (thisBoid.Position.y < yMin)
            {
                pushBack.y = 3;
            }
            else if (thisBoid.Position.y > yMax)
            {
                pushBack.y = -3;
            }
            if (thisBoid.Position.z < zMin)
            {
                pushBack.x = 3;
            }
            else if (thisBoid.Position.z > zMax)
            {
                pushBack.x = -3;
            }

            return pushBack.normalized;
        }
*/   
    }
}

using System;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Player
{
    public class TheKnight : IHollowKnight
    {
        private Texture2D sprite;

        private Vector2 position;
        private Vector2 velocity;

        private float moveSpeed = 200f;
        private float jumpSpeed = -350f;
        private float gravity = 900f;

        private bool isGrounded;

        public TheKnight(Texture2D sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;

            velocity = Vector2.Zero;
            isGrounded = false;
        }
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity.Y += gravity * dt;
            position += velocity * dt;

            float groundY = 400f;

            if (position.Y >= groundY)
            {
                position.Y = groundY;
                velocity.Y = 0;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }
        public void MoveRight()
        {
            velocity.X = moveSpeed;
        }
        public void MoveLeft()
        {
            velocity.X = -moveSpeed;
        }
        public void Jump()
        {
            if (isGrounded)
            {
                velocity.Y = jumpSpeed;
                isGrounded = false;
            }
        }
        public void StopMovingHorizontal()
        {
            velocity.X = 0;
        }
        public void Attack()
        {
            Console.WriteLine("Attack");
        }
    }
}
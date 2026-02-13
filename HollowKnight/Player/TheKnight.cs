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


        private bool isDamaged;
        private double damagedTimer;
        private double damagedDuration = 0.4;

        private int currentItem;


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

            if (isDamaged)
            {
                damagedTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (damagedTimer >= damagedDuration)
                {
                    isDamaged = false;
                    damagedTimer = 0;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Color tint = isDamaged ? Color.Red : Color.White;
            spriteBatch.Draw(sprite, position, tint);
        }
        public void MoveRight()
        {
            velocity.X = moveSpeed;
        }
        public void MoveLeft()
        {
            velocity.X = -moveSpeed;
        }
        public void MoveUp()
        {
            Console.WriteLine("Camera Move Up");
        }
        public void MoveDown()
        {
            Console.WriteLine("Camera Move Down");
        }
        public void TakeDamage()
        {
            Console.WriteLine("Knight took damage");
            isDamaged = true;
            damagedTimer = 0;
        }
        public void UseItem(int _itemNumber)
        {
            currentItem = _itemNumber;
            Console.WriteLine($"Using item #{currentItem}");
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

        public void StopMovingVertical()
        {
            velocity.Y = 0;
        }
        public void Attack()
        {
            Console.WriteLine("Attack");
        }
    }
}
/* Framestack.cs - written by Chris Smith (crs12b), first created 1/12/14
 * For use with Simplimation project.
 * Requires frame.cs to work correctly
 * 
 * Purpose: This class creates a LinkedList of frame objects.
 * It allows the main software to manipulate the list without
 * touching it directly.
 */

//required components
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

/*
 * namespace: Simplimation
 *
 * This sets the code to work with the project Simplimation, using within
 * it's namespace
 */

namespace Simplimation
{
    // Begin class Framestack
    public partial class FrameStack
    {


        // Create and intialize a new list of frame objects, call it "working" as in "current working list".
        public List<frame> working = new List<frame>();
        
        /* Function: append
         * Parameters: ins, frame object.
         * Purpose: Allows frame objects to be inserted into working linked-list
         * 
         */


        public void append(frame ins)
        {
            working.Add(ins);
        }

      
        /* Function: remover
       * Parameters: kill, int primitive
       * Purpose: Removes a frame from working link-listed based on int value passed in.
       * will not crash with try/catch block
       */
        public void remover(int kill)
        {
            try
            {
                working.RemoveAt(kill);
            }
            catch
            {
                // do not remove, no crash
            }
        }

       
        /* Function: clear
         * Parameters: N/A
         * Purpose: Clears working linked list, good for when starting new project.
         * 
         */
        public void clear()
        {
            working.Clear();
        }

       
        /* Function: move
        * Parameters: to, int primitive. x, ref: reference of frame object
        * Purpose: Takes current reference of frame object on storyboard, moves it
        * to position specified by to. Has safety measures to prevent crash.
        */
        public void move(int to, ref frame x)
        {
            try
            {
                frame copy = x;
                working.Remove(x);
                working.Insert(to, copy);
            }
            catch
            {
                // do not move, invalid parameters, no crash
            }

        }


    }
} // end class framestack, end code.

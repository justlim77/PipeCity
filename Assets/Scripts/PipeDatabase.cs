using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeDatabase : MonoBehaviour {

	public List<Pipe> pipes = new List<Pipe>();
	
	void Awake ()
	{
		pipes.Add (new Pipe ("empty", 0, 0, 0, Pipe.PipeType.Default, false, false, false, false, 0, 0, false, false, 100F));
		pipes.Add (new Pipe ("silver_I_pipe", 1, 10, 1, Pipe.PipeType.Silver, false, true, false, true, 2, 0, false, false, 100F));
		pipes.Add (new Pipe ("silver_L_pipe", 2, 10, 1, Pipe.PipeType.Silver, true, true, false, false, 2, 0, false, false, 100F));
		pipes.Add (new Pipe ("silver_T_pipe", 3, 20, 1, Pipe.PipeType.Silver, false, true, true, true, 3, 0, false, false, 100F));
		pipes.Add (new Pipe ("silver_X_pipe", 4, 30, 1, Pipe.PipeType.Silver, true, true, true, true, 4, 0, false, false, 100F));
		pipes.Add (new Pipe ("bronze_I_pipe", 5, 10, 1, Pipe.PipeType.Bronze, false, true, false, true, 2, 0, false, false, 100F));
		pipes.Add (new Pipe ("bronze_L_pipe", 6, 10, 1, Pipe.PipeType.Bronze, true, true, false, false, 2, 0, false, false, 100F));
		pipes.Add (new Pipe ("bronze_T_pipe", 7, 20, 1, Pipe.PipeType.Bronze, false, true, true, true, 3, 0, false, false, 100F));
		pipes.Add (new Pipe ("bronze_X_pipe", 8, 30, 1, Pipe.PipeType.Bronze, true, true, true, true, 4, 0, false, false, 100F));
	}
}

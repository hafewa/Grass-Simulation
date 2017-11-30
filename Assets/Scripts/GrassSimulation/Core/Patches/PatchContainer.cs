﻿using UnityEngine;

namespace GrassSimulation.Core.Patches
{
	public abstract class PatchContainer : ScriptableObject, IInitializableWithCtx
	{
		protected SimulationContext Ctx;

		public bool Init(SimulationContext context)
		{
			Ctx = context;
			return true;
		}

		public abstract void Destroy();

		public abstract Bounds GetBounds();

		public void Draw()
		{
			UpdatePerFrameData();
			DrawImpl();
		}

		protected abstract void DrawImpl();

		public abstract void SetupContainer();

		public void DrawGizmo()
		{
			if (Ctx.EditorSettings.EnableLodDistanceGizmo)
			{
				Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
				Gizmos.DrawWireSphere(Ctx.Camera.transform.position, Ctx.Settings.LodDistanceGeometryStart);
				Gizmos.DrawWireSphere(Ctx.Camera.transform.position, Ctx.Settings.LodDistanceGeometryEnd);
				Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
				Gizmos.DrawWireSphere(Ctx.Camera.transform.position, Ctx.Settings.LodDistanceBillboardCrossedStart);
				Gizmos.DrawWireSphere(Ctx.Camera.transform.position, Ctx.Settings.LodDistanceBillboardCrossedEnd);
				Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
				Gizmos.DrawWireSphere(Ctx.Camera.transform.position, Ctx.Settings.LodDistanceBillboardScreenStart);
				Gizmos.DrawWireSphere(Ctx.Camera.transform.position, Ctx.Settings.LodDistanceBillboardScreenEnd);
			}
			DrawGizmoImpl();
		}

		protected abstract void DrawGizmoImpl();

		public abstract void OnGUI();

		protected void UpdatePerFrameData()
		{
			//TODO: Maybe outsource all the computeshader data settings to its own class
			Ctx.GrassSimulationComputeShader.SetBool("applyTransition", Ctx.Settings.EnableHeightTransition);
			Ctx.GrassGeometry.SetVector("camPos", Ctx.Camera.transform.position);
			Ctx.GrassBillboardCrossed.SetVector("camPos", Ctx.Camera.transform.position);
			Ctx.GrassBillboardScreen.SetVector("camPos", Ctx.Camera.transform.position);
			Ctx.GrassBillboardScreen.SetVector("camUp", Ctx.Camera.transform.up);
			Ctx.GrassSimulationComputeShader.SetFloat("deltaTime", Time.deltaTime);
			Ctx.GrassSimulationComputeShader.SetVector("gravityVec", Ctx.Settings.Gravity);
			Ctx.GrassSimulationComputeShader.SetMatrix("viewProjMatrix",
				Ctx.Camera.projectionMatrix * Ctx.Camera.worldToCameraMatrix);
			Ctx.GrassSimulationComputeShader.SetFloats("camPos", Ctx.Camera.transform.position.x,
				Ctx.Camera.transform.position.y, Ctx.Camera.transform.position.z);
		}
	}
}
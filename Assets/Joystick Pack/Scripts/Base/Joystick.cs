﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JoystickLib
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public float Horizontal { get { return (snapX) ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; } }
        public float Vertical { get { return (snapY) ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; } }
        public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

        public float HandleRange
        {
            get { return handleRange; }
            set { handleRange = Mathf.Abs(value); }
        }

        public float DeadZone
        {
            get { return deadZone; }
            set { deadZone = Mathf.Abs(value); }
        }
        public bool InDeadZoneOnPointerUp { get; private set; }

        public float DeadZoneWhenPressed { get; private set; }
        public float DeadZoneWhenUnPressed { get; private set; }
        public bool Pressed { get; private set; }


        private float pressedTime;
        public float PressedTime => pressedTime;

        private float unPressedTime;
        public float UnPressedTime => unPressedTime;

        private float unPressedOrInDeadZoneTime;
        public float UnPressedOrInDeadZoneTime => unPressedOrInDeadZoneTime;

        public AxisOptions AxisOptions { get { return AxisOptions; } set { axisOptions = value; } }
        public bool SnapX { get { return snapX; } set { snapX = value; } }
        public bool SnapY { get { return snapY; } set { snapY = value; } }

        [SerializeField] private float handleRange = 1;
        [SerializeField] private float deadZone = 0;
        [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
        [SerializeField] private bool snapX = false;
        [SerializeField] private bool snapY = false;
        [SerializeField] private float deadZoneWhenPressed = 0f;
        [SerializeField] private float deadZoneWhenUnPressed = 0f;

        [SerializeField] protected RectTransform background = null;
        [SerializeField] private RectTransform handle = null;
        private RectTransform baseRect = null;

        private Canvas canvas;
        private Camera cam;

        private Vector2 input = Vector2.zero;

        public Action OnUp;
        public Action OnUpNotInDeadZone;
        public Action OnUpInDeadZone;

        public Action OnClamped;
        private bool clamped;

        protected virtual void Start()
        {
            HandleRange = handleRange;
            deadZone = deadZoneWhenUnPressed;
            DeadZone = deadZone;
            DeadZoneWhenPressed = deadZoneWhenPressed;
            DeadZoneWhenUnPressed = deadZoneWhenUnPressed;
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                Debug.LogError("The Joystick is not placed inside a canvas");

            Vector2 center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;
        }

        public bool InDeadZone()
        {
            if (Mathf.Abs(Horizontal) > DeadZone ||
                Mathf.Abs(Vertical) > DeadZone)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Pressed = true;
            InDeadZoneOnPointerUp = true;

            OnDrag(eventData);
        }

        private void Update()
        {
            if (Pressed)
            {
                if (!clamped)
                    StartCoroutine(OnClampedCor());
                else
                    OnClamped?.Invoke();
            }
            else
                clamped = false;

            TimeManagement();
        }

        private IEnumerator OnClampedCor()
        {
            yield return new WaitForEndOfFrame();

            if (Pressed)
            {
                clamped = true;
                OnClamped?.Invoke();
            }
        }

        private void TimeManagement()
        {
            if (Pressed)
            {
                pressedTime += Time.deltaTime;
                unPressedTime = 0;

                if (deadZone == deadZoneWhenUnPressed)
                {
                    unPressedOrInDeadZoneTime += Time.deltaTime;
                }
                else
                {
                    unPressedOrInDeadZoneTime = 0;
                }
            }
            else
            {
                unPressedTime += Time.deltaTime;
                unPressedOrInDeadZoneTime += Time.deltaTime;
                pressedTime = 0;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            input = (eventData.position - position) / (radius * canvas.scaleFactor);
            FormatInput();
            HandleInput(input.magnitude, input.normalized, radius, cam);
            handle.anchoredPosition = input * radius * handleRange;
        }

        protected virtual void HandleInput(float magnitude, Vector2 normalized, Vector2 radius, Camera cam)
        {
            if (magnitude > deadZone)
            {
                if (magnitude > 1)
                    input = normalized;

                deadZone = deadZoneWhenPressed;
            }
            else
            {
                input = Vector2.zero;
                deadZone = deadZoneWhenUnPressed;
            }
        }

        private void FormatInput()
        {
            if (axisOptions == AxisOptions.Horizontal)
                input = new Vector2(input.x, 0f);
            else if (axisOptions == AxisOptions.Vertical)
                input = new Vector2(0f, input.y);
        }

        private float SnapFloat(float value, AxisOptions snapAxis)
        {
            if (value == 0)
                return value;

            if (axisOptions == AxisOptions.Both)
            {
                float angle = Vector2.Angle(input, Vector2.up);
                if (snapAxis == AxisOptions.Horizontal)
                {
                    if (angle < 22.5f || angle > 157.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                else if (snapAxis == AxisOptions.Vertical)
                {
                    if (angle > 67.5f && angle < 112.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                return value;
            }
            else
            {
                if (value > 0)
                    return 1;
                if (value < 0)
                    return -1;
            }
            return 0;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            InDeadZoneOnPointerUp = InDeadZone();

            if (Horizontal != 0 || Vertical != 0)
                OnUpNotInDeadZone?.Invoke();

            if (Horizontal == 0 && Vertical == 0)
                OnUpInDeadZone?.Invoke();

            OnUp?.Invoke();

            Pressed = false;
            deadZone = deadZoneWhenUnPressed;

            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }

        protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
            {
                Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
                return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
            }
            return localPoint;
        }
    }

    public enum AxisOptions { Both, Horizontal, Vertical }
}
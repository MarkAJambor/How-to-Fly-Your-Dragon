using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using GPUInstancer;

public class DragonFlightController : MonoBehaviour {

    public float scaleFactor;
    public float timeLimit;
    public float multiplierDuration;
    public float orbPoints;
    public float orbTimeBonus;
    public float energyRegenerationRate;
    public float energyDrainRate;
    public float fireballSpeed;
    public float fireballEnergyCost;
    public float flamethrowerCostPerSecond;
    public float minimumEnergyForFlamethrower;
    public float recoverEnergyForFlamethrower;
    public float minimumEnergyForFlight;
    public float framesToTuck;
    public float defaultDrag;
    public float defaultForwardForce;
    public float baseRotationRate;
    public float baseRollAdjustment;
    public float basePitchAdjustment;
    public float baseYawAdjustment;
    public float liftConstant;
    public float negativeLiftConstant;
    public float yawConstant;
    public float rotationControlConstant;
    public float hoverConstant;
    public float rotationMax;
    public float liftMax;
    public float yawLiftMax;
    public float pitchStabilizationRate;
    public float yawStabilizationRate;
    public float velocity;
    public float lift;
    public float yawLift;
    public float forwardAngleToMotion;
    public float yawForwardAngleToMotion;
    public float rotationLift;
    public float forwardForce;
    public float hoverForce;
    public float pitch;
    public float yaw;
    public float roll;
    public float drag;
    public float yawCorrection;
    public float pitchCorrection;
    public float correctionRatio;
    public float flyInput;
    public float rightWingDisplacement;
    public float leftWingDisplacement;
    public float wingFlapSpeed;
    public float wingAngle;
    public float wingTilt;
    public float wingExtraTilt = 0;
    public float wingWobble;
    public float wingTime;
    public float flapRate;
    public float angleFromUp;
    public float angleFromForward;
    public float red;
    public float green;
    public float blue;
    public float points = 0;
    public float greenValue = 0;
    public float blueValue = 0;
    public float multiplierTime = 0;
    public float multiplierValue = 0;
    public float baseMultiplierValue;
    public float fireDelay = 1;
    public float lastFireTime = 0;
    public float flamethrowerStartTime = 0;
    public float flamethrowerLightStartIntensity;
    public float flamethrowerStopTime = 0;
    public float originalFireBarLifetime;
    public float elevation = 100;
    public float proximityPoints;
    public float distanceFromCenterSquared;
    public float distanceFromLavaFallsSquared;
    public float lastVelocity;
    public float originalFieldOfView;
    public float accelerationAtMaxZoom;
    public float maxFOVDeviation;
    public float FOVDamper;
    public float startTime;
    public float lastCheckpointIndicatorTime;
    public float checkpointIndicatorDelay;
    public float pointsAtStartOfRace;
    public float raceStartTime;
    public float elevationLiftValue;
    public float seaLevel;
    public float karmanLine;
    public float previousElevation;
    public float elevationGreenness;
    public float velocityGreenness;
    public float averageElevationGreenness;
    public float averageBlackout;
    public float averageAngleToForward;
    public float originalEnergyRegenRate;
    public float gameStartTime;
    public float timeLeft;
    public float framesSinceTuckStart;
    public float rollPoints;
    public float[] scalableVariables;
    public float[] elevationGreennessAverager;
    public float[] blackoutAverager;
    public float[] angleToForwardAverager;
    public int activeTerrain = 0;
    public int currentCheckpointGoal = 0;
    public int raceCheckpointArrayLength;
    public int currentSong;
    public int numberOfSongs;
    public int previousSecond = 1000;
    public Quaternion tempWingRotation;
    public Quaternion tempTailRotation;
    public Quaternion quat0;
    public Quaternion quat1;
    public Quaternion quat10;
    public GameData gameData;
    public PlayerData playerData;
    public Rigidbody selfRB;
    public Vector3 tempPosition;
    public Vector3 inputTorques = new Vector3(0, 0, 0);
    public Vector3 correctionTorques = new Vector3(0, 0, 0);
    public Vector3 originalHeadPosition;
    public Vector3 originalHeadSize;
    public Vector3 originalBodySize;
    public Vector3 originalWingSize;
    public Vector3 originalTailTrailSize;
    public Vector3 originalTailSize;
    public Vector3 flattenedForwardVector;
    public Vector3 worldUpVector = new Vector3(0, 1, 0);
    public Vector3 relativeUpVector;
    public Vector3 previousVelocityVector;
    public Vector3 rightWingTuckLocation;
    public Vector3 leftWingTuckLocation;
    public Vector3 rightWingTuckRotation;
    public Vector3 leftWingTuckRotation;
    public Vector3 originalRightWingLocation;
    public Vector3 originalLeftWingLocation;
    public Vector3 originalRightWingRotation;
    public Vector3 originalLeftWingRotation;
    public Vector3 goalRightWingLocation;
    public Vector3 goalLeftWingLocation;
    public Vector3 goalRightWingRotation;
    public Vector3 goalLeftWingRotation;
    public Vector3 startRightWingLocation;
    public Vector3 startLeftWingLocation;
    public Vector3 startRightWingRotation;
    public Vector3 startLeftWingRotation;
    public Vector3 previousUpVector;
    public bool flamethrowerExhausted = false;
    public bool gameIsPaused;
    public bool shouldAddPoints = false;
    public bool shouldAddMultiplier = false;
    public bool canUseFlamethrower = true;
    public bool wasUsingFlamethrower = false;
    public bool shouldSpawnCheckpointIndicators;
    public bool usingSkyLevel = false;
    public bool usingCaveLevel = false;
    public bool tucking = false;
    public bool untucking = false;
    public bool untucked = true;
    public bool tucked = false;
    public Slider wingLength;
    public Slider wingWidth;
    public Slider tailTrail;
    public Slider tailWidth;
    public Slider tailLength;
    public Slider bodySize;
    public Slider energySlider;
    public Slider velocitySlider;
    public Slider elevationSlider;
    public Slider musicVolumeSlider;
    public Slider gameVolumeSlider;
    public GameObject velocityIndicator;
    public GameObject forceIndicator;
    public GameObject fireball;
    public GameObject fireballSpawnLocation;
    public GameObject explosion;
    public GameObject tailComplete;
    public GameObject raceObjects;
    public GameObject checkpointExplosionEffect;
    public GameObject raceTrigger;
    public GameObject caveStartLocation;
    public GameObject skyStartLocation;
    public GameObject skyLevelAttractionPoint;
    public GameObject skyLight;
    public GameObject lavaFalls;
    public GameObject lavaFallsBase;
    public GameObject cannonAimLocation;
    public GameObject settingsMenu;
    public GameObject howToPlayMenu;
    public GameObject gameSummaryMenu;
    public GameObject leaderboardMenu;
    public ParticleSystem flamethrowerEffect;
    public ParticleSystem fireBarEffect;
    public AudioSource flamethrowerSoundEffect;
    public AudioSource wingFlapSoundEffect;
    public AudioSource pointsSoundEffect;
    public AudioSource[] songs;
    public Light flamethrowerLight;
    public Text timeDisplayText;
    public Text heightText;
    public Text velocityText;
    public Text angleOfAttackText;
    public Text pointsText;
    public Text adjustedScoreText;
    public Text multiplierRemainingText;
    public Text multiplierText;
    public Text highScoreText;
    public Text highScoreTimeText;
    public Text skyHighScoreText;
    public Text skyHighScoreTimeText;
    public Text adjustedHighScoreText;
    public Text raceRecordTimeText;
    public Text racePointsPerTimeText;
    public Text timeRemaining;
    public Text finalScore;
    public Text finalTime;
    public Toggle raceHintToggle;
    public Toggle musicToggle;
    public Toggle mutedToggle;
    public Toggle demonToggle;
    public Toggle spiderToggle;
    public Toggle crabToggle;
    public Toggle raceGoalToggle;
    public Toggle borkToggle;
    public Toggle tenKToggle;
    public Toggle hundredKToggle;
    public Toggle millionToggle;
    public Toggle tenMinuteToggle;
    public Toggle twentyMinuteToggle;
    public GameObject gameOverPanel;
    public GameObject newHighscore;
    public GameObject newRecord;
    public GameObject newAdjustedHighscore;
    public GameObject pauseMenu;
    public GameObject leftWing;
    public GameObject rightWing;
    public GameObject body;
    public GameObject tailTrailObject;
    public GameObject tail;
    public GameObject head;
    public GameObject headPiece;
    public GameObject bodyPiece;
    public GameObject tailTrailPiece;
    public GameObject tailPiece;
    public GameObject leftWingPiece;
    public GameObject rightWingPiece;
    public GameObject raceSpawnLocation1;
    public GameObject nextCheckpointIndicator;
    public GameObject pausedIndicator;
    public GameObject restartButton;
    public GameObject flockController;
    public GameObject currentFlockController;
    public GameObject flockSpawner;
    public GameObject[] flocks;
    public GameObject[] raceCheckpoints;
    public GameObject[] raceShields;
    public Resolution[] resolutions;
    public Dropdown resolutionSelecter;
    public Dropdown locationSelecter;
    public Camera playerCamera;
    public Color tempColor;
    public Color elevationSliderColor;
    public Color velocitySliderColor;
    public Image elevationSliderImage;
    public Image velocitySliderImage;
    public Image blackoutPanel;
    public Image snowflakePanel;
    public RawImage angleOfAttackArrowImage;
    public RectTransform angleOfAttackArrow;
    public Material milkyWay;
    public Material caveSkybox;
    public Material nextColor;
    public Material secondNextColor;
    public Material thirdNextColor;
    public Material[] orbMaterials;
    public OrbSpawner orbSpawner;
    public GameObject indicator;
    public Terrain[] terrains;
    public RaycastHit[] raycasts = new RaycastHit[18];
    public Vector3[] raycastDirections = new Vector3[18];
    public LayerMask flamethrowerMask = (1 << 0) | (1 << 9) | (1 << 12) | (1 << 13) | (1 << 16) | (1 << 20);
    public LayerMask proximityCheckerMask = (1 << 0) | (1 << 9);
    public StoneMonsterController stoneMonster;
    public ShellCrabController[] shellCrabArray;
    public SpiderController spider;
    public PlanetOrbSpawner planetOrbSpawner;
    public VikingShipController[] ships;
    public AudioSource deathSound;
    public AudioSource wind1;
    public AudioSource wind2;
    public AudioSource beep;
    public Highscores highscoreAdder;
    public TrailRenderer rightWingTrail;
    public TrailRenderer leftWingTrail;
    public Color wingTipTrailColor;


    // Use this for initialization
    void Start()
    {
        //initialize variable values, build map, and set start positions
        Application.targetFrameRate = 144;
        Cursor.visible = true;
        originalRightWingLocation = rightWing.transform.localPosition;
        originalRightWingRotation = rightWing.transform.localRotation.eulerAngles;
        originalLeftWingLocation = leftWing.transform.localPosition;
        originalLeftWingRotation = leftWing.transform.localRotation.eulerAngles;
        liftMax *= scaleFactor;
        yawLiftMax *= scaleFactor;
        rotationMax *= Mathf.Pow(scaleFactor, 3);
        multiplierValue = 1;
        baseMultiplierValue = multiplierValue;
        //defaultForwardForce *= scaleFactor;
        //hoverForce *= scaleFactor;
        //hoverConstant *= scaleFactor;
        baseRotationRate *= Mathf.Pow(scaleFactor, 2);
        Vector3 tempScale = this.transform.localScale;
        tempScale *= scaleFactor;
        this.transform.localScale = tempScale;
        originalEnergyRegenRate = energyRegenerationRate;

        flocks = new GameObject[3];
        ships = FindObjectsOfType<VikingShipController>();
        orbSpawner = FindObjectOfType<OrbSpawner>();
        planetOrbSpawner = FindObjectOfType<PlanetOrbSpawner>();
        raycastDirections[0] = new Vector3(0, 0, 1);
        raycastDirections[1] = new Vector3(0, 0, -1);
        raycastDirections[2] = new Vector3(1, 0, 0);
        raycastDirections[3] = new Vector3(-1, 0, 0);
        raycastDirections[4] = new Vector3(0, 1, 0);
        raycastDirections[5] = new Vector3(0, -1, 0);
        raycastDirections[6] = new Vector3(1, 0, 1);
        raycastDirections[7] = new Vector3(-1, 0, 1);
        raycastDirections[8] = new Vector3(1, 0, -1);
        raycastDirections[9] = new Vector3(-1, 0, 1);
        raycastDirections[10] = new Vector3(0, 1, 1);
        raycastDirections[11] = new Vector3(0, -1, 1);
        raycastDirections[12] = new Vector3(0, 1, -1);
        raycastDirections[13] = new Vector3(0, -1, -1);
        raycastDirections[14] = new Vector3(1, 1, 0);
        raycastDirections[15] = new Vector3(1, -1, 0);
        raycastDirections[16] = new Vector3(-1, 1, 0);
        raycastDirections[17] = new Vector3(-1, -1, 0);
        this.GetComponentInChildren<Camera>().layerCullSpherical = true;
        float[] distances = new float[32];
        distances[0] = this.GetComponentInChildren<Camera>().farClipPlane;
        distances[8] = 25000;
        rollPoints = 0;
        GetComponentInChildren<Camera>().layerCullDistances = distances;
        playerCamera = GetComponentInChildren<Camera>();
        originalFieldOfView = playerCamera.fieldOfView;
        raceObjects.SetActive(false);
        this.loadPlayerData();
        this.loadGameData();
        this.setOriginalPartSizes();
        this.setSliders();
        this.updatePartSizes();
        locationSelecter.value = playerData.location;
        this.setLocation();
        if (playerData.raceIndicator)
        {
            raceHintToggle.isOn = true;
        }
        else
        {
            raceHintToggle.isOn = false;
        }
        this.resetLevel();
        wingAngle = 0;
        wingTime = 0;
        leftWingDisplacement = leftWing.transform.localPosition.x;
        rightWingDisplacement = rightWing.transform.localPosition.x;
        originalHeadPosition = head.transform.localPosition;
        flamethrowerLightStartIntensity = flamethrowerLight.intensity;
        originalFireBarLifetime = 2f;
        currentSong = 100;
        numberOfSongs = 3;

        musicVolumeSlider.value = playerData.musicVolume;
        gameVolumeSlider.value = playerData.gameVolume;
        resolutions = Screen.resolutions;
        resolutionSelecter.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height && playerData.resolution == 123456789)
            {
                playerData.resolution = i;
            }
        }
        resolutionSelecter.AddOptions(options);
        this.setResolution(playerData.resolution);
        if (playerData.resolution < resolutions.Length)
        {
            resolutionSelecter.value = playerData.resolution;
        }
        if (playerData.playerID == "0123456789" || playerData.playerID == null)
        {
            playerData.playerID = "Player";
            for (int i = 0; i < 20; i++)
            {
                playerData.playerID = String.Concat(playerData.playerID, UnityEngine.Random.Range(0, 10).ToString());
            }
            this.savePlayerData();
        }
        this.setGoalToggles();
        this.giveInitialBoost();
        previousUpVector = this.transform.up;
    }

    // Update is called once per frame
    void Update ()
    {
        //Debug.Log("Pitch: " + Input.GetAxis("Pitch") + " Yaw: " + Input.GetAxis("Yaw") + " Roll: " + Input.GetAxis("Roll"));
        
        //enable/disable pause menu
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("StartButton"))
        {
            if (gameIsPaused)
            {
                if (gameOverPanel.activeSelf)
                {
                    gameOverPanel.SetActive(false);
                }
                else
                {
                    if (pausedIndicator.gameObject.activeSelf)
                    {
                        this.resumeGame();
                    }
                }
            }
            else
            {
                pausedIndicator.gameObject.SetActive(true);
                this.pauseGame();
            }
        }

        //handle menu navigation
        if (Input.GetButtonDown("BButton") && (settingsMenu.activeSelf || howToPlayMenu.activeSelf || leaderboardMenu.activeSelf) && !gameSummaryMenu.activeSelf)
        {
            //disable sub-menus
            settingsMenu.SetActive(false);
            howToPlayMenu.SetActive(false);
            leaderboardMenu.SetActive(false);
        }
        else if (Input.GetButtonDown("BButton") && !gameSummaryMenu.activeSelf)
        {
            howToPlayMenu.SetActive(true);
        }
        if (Input.GetButtonDown("YButton") && !settingsMenu.activeSelf && !howToPlayMenu.activeSelf && !gameSummaryMenu.activeSelf)
        {
            settingsMenu.SetActive(true);
        }
        if (Input.GetButtonDown("XButton") && !settingsMenu.activeSelf && !howToPlayMenu.activeSelf && restartButton.activeSelf && !gameSummaryMenu.activeSelf)
        {
            this.resetLevel();
        }
        if (Input.GetButtonDown("AButton") && !settingsMenu.activeSelf && !howToPlayMenu.activeSelf && !gameSummaryMenu.activeSelf)
        {
            this.resumeGame();
        }
        else if (Input.GetButtonDown("AButton") && gameSummaryMenu.activeSelf)
        {
            gameSummaryMenu.SetActive(false);
        }

        //var main = fireBarEffect.main;
        //main.startLifetime = originalFireBarLifetime * energySlider.maxValue / energySlider.value;
        //Debug.Log(main.startLifetime.ToString() + " and " + fireBarEffect.main.startLifetime);
        
        //start energy bar
        foreach (ParticleSystem temp in fireBarEffect.GetComponentsInChildren<ParticleSystem>())
        {
            temp.startLifetime = originalFireBarLifetime * energySlider.value / energySlider.maxValue;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //artificially end the game
            this.endGame();
        }

        //spawn checkpoint indicators during race
        if (playerData.raceIndicator && !usingCaveLevel && !usingSkyLevel)
        {
            if (Time.time - lastCheckpointIndicatorTime > checkpointIndicatorDelay)
            {
                int length = 0;
                GameObject[] remainingCheckpoints = new GameObject[(raceCheckpointArrayLength - currentCheckpointGoal)];
                for (int i = 0; i < raceCheckpointArrayLength - currentCheckpointGoal; i++)
                {
                    remainingCheckpoints[i] = raceCheckpoints[currentCheckpointGoal + i];
                    length++;
                }
                lastCheckpointIndicatorTime = Time.time;
                GameObject temp = Instantiate(nextCheckpointIndicator);
                temp.transform.position = fireballSpawnLocation.transform.position;
                temp.transform.rotation = fireballSpawnLocation.transform.rotation;
                temp.GetComponent<Rigidbody>().velocity = selfRB.velocity * 2;
                temp.GetComponent<NextCheckpointIndicator>().checkpoints = remainingCheckpoints;
                temp.GetComponent<NextCheckpointIndicator>().checkpointsLength = length;
            }
        }
    }

    private void FixedUpdate()
    {
        //play song tracks
        if (!songs[0].isPlaying && !songs[1].isPlaying && !songs[2].isPlaying && playerData.musicEnabled)
        {
            this.playNextTrack();
        }
        for (int i = 0; i < flocks.Length; i++)
        {
            if (flocks[i] == null)
            {
                this.spawnFlock(i);
            }
        }
        //update dragoids flocks
        foreach (GameObject obj in flocks)
        {
            if (usingCaveLevel && (obj.transform.position - this.transform.position).magnitude > 2500)
            {
                obj.GetComponentInChildren<FlockController>().destroyDragoids();
                DestroyImmediate(obj);
            }
            else if (usingSkyLevel && (obj.transform.position - this.transform.position).magnitude > 3000)
            {
                obj.GetComponentInChildren<FlockController>().destroyDragoids();
                DestroyImmediate(obj);
            }
        }
        //update terrain and elevation
        this.setTerrain();
        elevation = selfRB.transform.position.y - terrains[activeTerrain].SampleHeight(this.transform.position);

        //update orb colors
        //determine angle from up
        relativeUpVector = Vector3.Cross(Vector3.Cross(this.transform.forward, worldUpVector), this.transform.forward);
        angleFromUp = Vector3.SignedAngle(this.transform.up, relativeUpVector, this.transform.forward);
        if (angleFromUp < 0)
        {
            angleFromUp += 360;
        }
        //set up color
        red = 1 - Mathf.Abs(3 * angleFromUp - 720) / 360;
        blue = 1 - Mathf.Abs(3 * angleFromUp - 360) / 360;
        green = 1 - Mathf.Abs(3 * angleFromUp) / 360;
        if (angleFromUp > 180)
        {
            green = 1 - Mathf.Abs(3 * angleFromUp - 1080) / 360;
        }
        //set orb colors offset by 120 degres
        tempColor.r = red;
        tempColor.b = blue;
        tempColor.g = green;
        orbMaterials[0].color = tempColor;
        tempColor.r = green;
        tempColor.b = red;
        tempColor.g = blue;
        orbMaterials[1].color = tempColor;
        tempColor.r = blue;
        tempColor.b = green;
        tempColor.g = red;
        orbMaterials[2].color = tempColor;
        
        //get inputs
        if (energySlider.value > minimumEnergyForFlight)
        {
            flyInput = -Input.GetAxis("Fly");
        }
        else
        {
            flyInput = 0;
        }
        pitch = Input.GetAxis("Pitch") * basePitchAdjustment;
        yaw = Input.GetAxis("Yaw") * baseYawAdjustment;
        roll = -Input.GetAxis("Roll") * baseRollAdjustment;
        drag = defaultDrag;
        if (energySlider.value > minimumEnergyForFlight)
        {
            if (flyInput > 0)
            {
                forwardForce = flyInput * defaultForwardForce;
                hoverForce = 0;
            }
            else
            {
                forwardForce = 0;
                drag += -flyInput;
                hoverForce = -flyInput * hoverConstant * 2;
            }
        }
        else
        {
            forwardForce = 0;
        }

        //calculate flight physics
        //Debug.Log(Vector3.SignedAngle(this.transform.forward, selfRB.velocity, this.transform.right));
        forwardAngleToMotion = Vector3.SignedAngle(this.transform.forward, selfRB.velocity, this.transform.right);
        yawForwardAngleToMotion = -Vector3.SignedAngle(this.transform.forward, selfRB.velocity, this.transform.up);
        lift = liftConstant * Mathf.Pow(selfRB.velocity.magnitude, 2) * Mathf.Sin(Mathf.Deg2Rad * forwardAngleToMotion / 2);
        yawLift = yawConstant * Mathf.Pow(selfRB.velocity.magnitude, 2) * Mathf.Sin(Mathf.Deg2Rad * yawForwardAngleToMotion / 2);
        if (lift < 0)
        {
            lift /= liftConstant;
            lift *= negativeLiftConstant;
        }
        rotationLift = rotationControlConstant * Mathf.Pow(selfRB.velocity.magnitude, 2) + baseRotationRate;
        if (lift > liftMax)
        {
            lift = liftMax;
        }
        if (yawLift > yawLiftMax)
        {
            yawLift = yawLiftMax;
        }
        if (rotationLift > rotationMax)
        {
            rotationLift = rotationMax;
        }
        pitch *= rotationLift;
        yaw *= rotationLift;
        roll *= (rotationLift * 2);
        inputTorques.x = -pitch;
        inputTorques.y = yaw;
        inputTorques.z = roll;

        if (selfRB.velocity.magnitude > 7.5f)
        {
            pitchCorrection = -Vector3.SignedAngle(this.transform.forward, selfRB.velocity, this.transform.right) * pitchStabilizationRate * rotationLift * correctionRatio;
            yawCorrection = Vector3.SignedAngle(this.transform.forward, selfRB.velocity, this.transform.up) * yawStabilizationRate * rotationLift * correctionRatio;
        }
        else
        {
            pitchCorrection = 0;
            yawCorrection = 0;
        }
        correctionTorques.x = -pitchCorrection;
        correctionTorques.y = yawCorrection;

        //apply motion forces
        if (usingSkyLevel)
        {
            if ((this.transform.position - skyLevelAttractionPoint.transform.position).magnitude > (seaLevel + karmanLine))
            {
                elevationLiftValue = 0;
            }
            else
            {
                elevationLiftValue = Mathf.Max(0, Mathf.Pow((1 - (((this.transform.position - skyLevelAttractionPoint.transform.position).magnitude - seaLevel) / karmanLine)), 5));
            }
        }
        else
        {
            elevationLiftValue = 1;
        }

        //apply gravitation
        //Debug.Log("elevation lift value: " + elevationLiftValue.ToString("0.000"));
        if (usingSkyLevel)
        {
            selfRB.AddForce((skyLevelAttractionPoint.transform.position - this.transform.position).normalized * 9.8f, ForceMode.Acceleration);
        }
        if (untucked)
        {
            selfRB.AddForce(this.transform.up * lift * elevationLiftValue, ForceMode.Force);
            selfRB.AddForce(this.transform.up * hoverForce * elevationLiftValue, ForceMode.Force);
            selfRB.AddForce(this.transform.right * yawLift * elevationLiftValue, ForceMode.Force);
            selfRB.AddForce(this.transform.forward * forwardForce * elevationLiftValue, ForceMode.Force);
        }
        if (flyInput >= 0)
        {
            selfRB.drag = 0;
            selfRB.AddForce(-this.transform.forward * drag * Mathf.Pow(selfRB.transform.InverseTransformDirection(selfRB.velocity).z, 2) * elevationLiftValue, ForceMode.Force);
        }
        else
        {
            //apply drag
            selfRB.drag = 0;
            selfRB.AddForce(-this.transform.forward * drag * 0.05f * Mathf.Pow(selfRB.transform.InverseTransformDirection(selfRB.velocity).z * elevationLiftValue, 1.7f), ForceMode.Force);
        }
        selfRB.AddRelativeTorque(inputTorques * elevationLiftValue);


        //stablize flight towards forward
        selfRB.AddRelativeTorque(correctionTorques * elevationLiftValue);
        velocityIndicator.transform.position = selfRB.transform.position + selfRB.velocity;
        forceIndicator.transform.position = selfRB.transform.position + this.transform.forward * forwardForce;

        //update wing tip trail color
        wingTipTrailColor = rightWingTrail.startColor;
        wingTipTrailColor.a = (selfRB.velocity.magnitude - 200) / 500;
        rightWingTrail.startColor = wingTipTrailColor;
        leftWingTrail.startColor = wingTipTrailColor;
        //flap wings
        wingFlapSpeed = Mathf.Abs(flyInput);
        if (wingFlapSpeed > 0.05f && wingFlapSpeed < 0.25f)
        {
            wingFlapSpeed = 0.25f;
        }
        wingTime += flapRate * wingFlapSpeed * Time.deltaTime;

        //normalize wing angle without input
        if (flyInput == 0)
        {
            if (Mathf.Abs(wingAngle) > 5 || wingTilt > 5)
            {
                wingTime += Time.deltaTime * (Mathf.Abs(wingAngle) + wingTilt) / 10;            
            }
        }
        wingExtraTilt += Input.GetAxis("Roll") * Time.deltaTime * 100;
        if (Mathf.Abs(Input.GetAxis("Roll")) <= 0.1f)
        {
            if (Mathf.Abs(wingExtraTilt) >= 2)
            {
                if (wingExtraTilt > 0)
                {
                    wingExtraTilt -= Time.deltaTime * 100;
                }
                else if (wingExtraTilt < 0)
                {
                    wingExtraTilt += Time.deltaTime * 100;
                }
            }
        }
        else
        {
            wingExtraTilt = Mathf.Clamp(wingExtraTilt, -45 * Mathf.Abs(Input.GetAxis("Roll")), 45 * Mathf.Abs(Input.GetAxis("Roll")));
        }
        wingAngle = 52 * Mathf.Sin(wingTime) - 7;
        wingTilt = 52 * Mathf.Cos(wingTime) - 7;
        if (wingAngle < -1 && wingFlapSpeed > 0.05f && !untucking && !tucking)
        {
            wingFlapSoundEffect.pitch = wingFlapSpeed * 2;
            if (!wingFlapSoundEffect.isPlaying)
            {
                //play wing flap sound effect
                
                wingFlapSoundEffect.Play();
            }
        }
        if (wingTilt < 0)
        {
            wingTilt = 0;
        }

        //apply wing-tuck animation
        if (Input.GetAxis("Hover") >= 0.9f && Input.GetAxis("FlyForward") >= 0.9f && !tucking && !tucked)
        {
            //executes only on the first frame of both buttons being pressed
            tucking = true;
            untucking = false;
            untucked = false;
            framesSinceTuckStart = 0;
            //record goal and starting locations and rotations of wings
            goalRightWingLocation = rightWingTuckLocation;
            goalRightWingRotation = rightWingTuckRotation;
            goalLeftWingLocation = leftWingTuckLocation;
            goalLeftWingRotation = leftWingTuckRotation;
            startLeftWingLocation = leftWing.transform.localPosition;
            startLeftWingRotation = leftWing.transform.localRotation.eulerAngles;
            startRightWingLocation = rightWing.transform.localPosition;
            startRightWingRotation = rightWing.transform.localRotation.eulerAngles;
        }
        if ((Input.GetAxis("Hover") < 0.9f || Input.GetAxis("FlyForward") < 0.9f) && !untucking && !untucked)
        {
            tucking = false;
            untucking = true;
            framesSinceTuckStart = 0;
            //record goal and starting locations and rotations of wings
            goalRightWingLocation = originalRightWingLocation;
            goalRightWingRotation = originalRightWingRotation;
            goalLeftWingLocation = originalLeftWingLocation;
            goalLeftWingRotation = originalLeftWingRotation;
            startLeftWingLocation = leftWing.transform.localPosition;
            startLeftWingRotation = leftWing.transform.localRotation.eulerAngles;
            startRightWingLocation = rightWing.transform.localPosition;
            startRightWingRotation = rightWing.transform.localRotation.eulerAngles;
        }

        if (untucked)
        {
            wingWobble = UnityEngine.Random.Range(-60, 60) * Time.deltaTime * selfRB.velocity.magnitude / 250;
            //set right wing
            tempWingRotation = rightWing.transform.localRotation;
            tempWingRotation = Quaternion.Euler(Mathf.Clamp(-wingTilt + wingExtraTilt, -30, 0) + wingWobble, 0, wingAngle);
            rightWing.transform.localRotation = tempWingRotation;
            tempPosition = rightWing.transform.localPosition;
            tempPosition.x = rightWingDisplacement * Mathf.Cos(wingAngle * Mathf.Deg2Rad);
            tempPosition.y = rightWingDisplacement * Mathf.Sin(wingAngle * Mathf.Deg2Rad);
            rightWing.transform.localPosition = tempPosition;
            //set left wing
            tempWingRotation = leftWing.transform.localRotation;
            tempWingRotation = Quaternion.Euler(-Mathf.Clamp(wingTilt + wingExtraTilt, 0, 30) + wingWobble, 0, -wingAngle);
            leftWing.transform.localRotation = tempWingRotation;
            tempPosition = leftWing.transform.localPosition;
            tempPosition.x = leftWingDisplacement * Mathf.Cos(-wingAngle * Mathf.Deg2Rad);
            tempPosition.y = leftWingDisplacement * Mathf.Sin(-wingAngle * Mathf.Deg2Rad);
            leftWing.transform.localPosition = tempPosition;
        }

        if (tucking || untucking)
        {
            wingFlapSoundEffect.Stop();
            rightWing.transform.localPosition = startRightWingLocation - (framesSinceTuckStart / framesToTuck) * (startRightWingLocation - goalRightWingLocation);
            rightWing.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(startRightWingRotation), Quaternion.Euler(goalRightWingRotation), (framesSinceTuckStart / framesToTuck));
            leftWing.transform.localPosition = startLeftWingLocation - (framesSinceTuckStart / framesToTuck) * (startLeftWingLocation - goalLeftWingLocation);
            leftWing.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(startLeftWingRotation), Quaternion.Euler(goalLeftWingRotation), (framesSinceTuckStart / framesToTuck));
            framesSinceTuckStart++;
            if (framesSinceTuckStart == framesToTuck)
            {
                if (tucking)
                {
                    tucking = false;
                    tucked = true;
                    untucking = false;
                    untucked = false;
                    rightWing.transform.localPosition = goalRightWingLocation;
                    rightWing.transform.localRotation = Quaternion.Euler(goalRightWingRotation);
                    leftWing.transform.localPosition = goalLeftWingLocation;
                    leftWing.transform.localRotation = Quaternion.Euler(goalLeftWingRotation);
                }
                if (untucking)
                {
                    untucking = false;
                    untucked = true;
                    tucking = false;
                    tucked = false;
                }
            }
        }

        //set tail position
        tempTailRotation = Quaternion.RotateTowards(tailComplete.transform.localRotation, Quaternion.Euler(30 * Input.GetAxis("Pitch"), 0, -30 * Input.GetAxis("Roll")), 1);
        tailComplete.transform.localRotation = tempTailRotation;

        //make body bounce
        tempPosition = body.transform.localPosition;
        tempPosition.y = Mathf.Sin(-wingAngle * Mathf.Deg2Rad)/2;
        body.transform.localPosition = tempPosition;

        tempPosition = originalHeadPosition;
        tempPosition.y -= Mathf.Sin(-wingAngle * Mathf.Deg2Rad) / 2;
        head.transform.localPosition = tempPosition;

        //activate attacks
        if (Input.GetButton("Fire1") && !Input.GetButton("Fire2") && Time.time > lastFireTime + fireDelay && energySlider.value > fireballEnergyCost)
        {
            //attack 1
            GameObject temp = Instantiate(fireball);
            temp.transform.position = fireballSpawnLocation.transform.position;
            temp.transform.rotation = fireballSpawnLocation.transform.rotation;
            temp.GetComponentInChildren<Rigidbody>().velocity = selfRB.velocity + fireballSpawnLocation.transform.forward * fireballSpeed;
            lastFireTime = Time.time;
            energySlider.value -= fireballEnergyCost;
            foreach (ParticleSystem PS in fireball.GetComponentsInChildren<ParticleSystem>())
            {
                PS.Play();
            }
        }

        if (energySlider.value <= minimumEnergyForFlamethrower)
        {
            flamethrowerExhausted = true;
        }
        if (energySlider.value >= recoverEnergyForFlamethrower)
        {
            flamethrowerExhausted = false;
        }
        if (Input.GetButton("Fire2") && !Input.GetButton("Fire1") && canUseFlamethrower && !flamethrowerExhausted)
        {
            flamethrowerLight.enabled = true;
            flamethrowerLight.intensity = flamethrowerLightStartIntensity;
            if (!flamethrowerSoundEffect.isPlaying)
            {
                flamethrowerSoundEffect.time = 0;
                flamethrowerSoundEffect.Play();
                flamethrowerStartTime = Time.time;
                wasUsingFlamethrower = true;
            }
            energySlider.value -= flamethrowerCostPerSecond * Time.deltaTime;
            flamethrowerEffect.enableEmission = true;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(fireballSpawnLocation.transform.position, fireballSpawnLocation.transform.TransformDirection(Vector3.forward), out hit, Mathf.Min((Time.time - flamethrowerStartTime) * 500 / 2, 500), flamethrowerMask))
            {
                if (hit.collider.tag == "Point Claw")
                {
                    greenValue = hit.collider.GetComponentInChildren<MeshRenderer>().material.color.g;
                    blueValue = hit.collider.GetComponentInChildren<MeshRenderer>().material.color.b;
                    shouldAddPoints = true;
                    this.playPointsSound(greenValue, blueValue);
                    Destroy(hit.collider.gameObject);
                    orbSpawner.numberOfOrbs--;
                    GameObject explosionTemp = Instantiate(explosion);
                    explosionTemp.transform.position = hit.point;
                    Destroy(explosionTemp, 5);
                }

                if (hit.collider.tag == "Planet Orb")
                {
                    greenValue = 1;
                    blueValue = 0;
                    shouldAddPoints = true;
                    Destroy(hit.collider.gameObject);
                    planetOrbSpawner.spawnNextOrb();
                    this.playPointsSound(1, 0);
                    GameObject explosionTemp = Instantiate(explosion);
                    explosionTemp.transform.position = hit.point;
                    Destroy(explosionTemp, 5);
                    Destroy(hit.collider.gameObject);
                }

                if (hit.collider.tag == "Multiplier")
                {
                    if (usingSkyLevel)
                    {
                        baseMultiplierValue += 0.5f;
                        planetOrbSpawner.spawnNextMultiplier();
                    }
                    else if (usingCaveLevel)
                    {
                        shouldAddMultiplier = true;
                        orbSpawner.numberOfMultipliers--;
                    }
                    this.playMultiplierSound();
                    Destroy(hit.collider.transform.parent.gameObject);
                    GameObject explosionTemp = Instantiate(explosion);
                    explosionTemp.transform.position = hit.point;
                    Destroy(explosionTemp, 5);
                }
                //Debug.Log("raycast hit: " + hit.collider.tag);
                if (hit.collider.tag == "Stone Monster")
                {
                    hit.collider.GetComponentInParent<StoneMonsterController>().shouldTakeFlamethrowerDamage = true;
                }

                if (hit.collider.tag == "ShellCrab")
                {
                    hit.collider.GetComponentInParent<ShellCrabController>().shouldTakeFlamethrowerDamage = true;
                }

                if (hit.collider.tag == "Spider")
                {
                    hit.collider.GetComponentInParent<SpiderController>().shouldTakeFlamethrowerDamage = true;
                }

                if (hit.collider.tag == "Ship")
                {
                    hit.collider.GetComponentInParent<VikingShipController>().shouldTakeFlamethrowerDamage = true;
                }

                if (hit.collider.tag == "SpiderShot")
                {
                    hit.collider.GetComponent<SpiderShotController>().destroyShot();
                    points += 1;
                }
            }
            if (Time.time - flamethrowerStartTime > flamethrowerSoundEffect.clip.length)
            {
                canUseFlamethrower = false;
            }
        }
        else
        {
            if (wasUsingFlamethrower)
            {
                if (flamethrowerSoundEffect.time < 5f)
                {
                    flamethrowerSoundEffect.time = 5f;
                }
                wasUsingFlamethrower = false;
                flamethrowerStopTime = Time.time;
            }
            //flamethrowerSoundEffect.Stop();
            flamethrowerEffect.enableEmission = false;
            if (flamethrowerLight.enabled)
            {
                flamethrowerLight.intensity = flamethrowerLightStartIntensity * (1 - (Time.time - flamethrowerStopTime) / 1);
            }
            if (flamethrowerLight.intensity <= 0)
            {
                flamethrowerLight.enabled = false;
            }
        }

        if (!Input.GetButton("Fire2"))
        {
            canUseFlamethrower = true;
        }

        //add multiplier
        if (shouldAddMultiplier)
        {
            if (multiplierTime < Time.time)
            {
                multiplierTime = Time.time + multiplierDuration;
            }
            else
            {
                multiplierTime += multiplierDuration;
            }
            shouldAddMultiplier = false;
        }
        if (Time.time < multiplierTime)
        {
            multiplierRemainingText.gameObject.SetActive(true);
            multiplierRemainingText.text = (multiplierTime - Time.time).ToString("0");
        }
        else
        {
            multiplierRemainingText.gameObject.SetActive(false);
        }

        //add points
        if (multiplierTime > Time.time)
        {
            multiplierValue = baseMultiplierValue * 3;
        }
        else
        {
            multiplierValue = baseMultiplierValue;
        }
        if (shouldAddPoints)
        {
            points += orbPoints * Mathf.Clamp(greenValue, 0, 1) * 2 * multiplierValue;
            points += orbPoints * Mathf.Clamp(blueValue, 0, 1) * multiplierValue;
            timeLeft += orbTimeBonus * Mathf.Clamp(greenValue, 0, 1) * 2;
            timeLeft += orbTimeBonus * Mathf.Clamp(blueValue, 0, 1);
            shouldAddPoints = false;
        }
        multiplierText.text = "X" + multiplierValue.ToString("0.0");
        //points += (1 - wingFlapSpeed) * Time.deltaTime * multiplierValue * Mathf.Max(1, Mathf.Sqrt(250 / elevation));
        if (usingSkyLevel)
        {
            elevation = (this.transform.position - skyLevelAttractionPoint.transform.position).magnitude - seaLevel;
        }
        pointsText.text = "Points: " + points.ToString("0.0");
        heightText.text = "Elevation: " + (elevation).ToString("0");
        velocityText.text = "Velocity: " + selfRB.velocity.magnitude.ToString("0");
        angleOfAttackText.text = "Angle of attack: " + forwardAngleToMotion.ToString("0");
        timeDisplayText.text = "Time: " + (Time.time - startTime).ToString("0.0");
        adjustedScoreText.text = "Adjusted Score: " + (points * points / (Time.time - startTime)).ToString("0");
        angleOfAttackArrow.rotation = Quaternion.Euler(0, 0, averageAngleToForward);
        angleOfAttackArrowImage.color = new Color(1, Mathf.Max(0, (1 - Mathf.Abs(averageAngleToForward)/35)), Mathf.Max(0, (1 - Mathf.Abs(averageAngleToForward) / 35)));

        //set elevation and velocity sliders
        if (usingCaveLevel)
        {
            elevationSlider.gameObject.SetActive(true);
            elevationSlider.value = Mathf.Atan(elevation / 400) * 2 / Mathf.PI;
        }
        else if (usingSkyLevel)
        {
            elevationSlider.gameObject.SetActive(true);
            elevationSlider.value = Mathf.Atan(elevation / 800) * 2 / Mathf.PI;
        }
        else
        {
            elevationSlider.gameObject.SetActive(false);
        }
        elevationGreenness = Mathf.Atan((elevation - previousElevation) / 5) * 2 / Mathf.PI;
        averageElevationGreenness = 0;
        for (int i = 0; i < elevationGreennessAverager.Length; i++)
        {
            averageElevationGreenness += elevationGreennessAverager[i];
            if (i < elevationGreennessAverager.Length - 1)
            {
                elevationGreennessAverager[i] = elevationGreennessAverager[i + 1];
            }
            else
            {
                elevationGreennessAverager[i] = elevationGreenness;
            }
        }
        averageElevationGreenness /= elevationGreennessAverager.Length;
        elevationSliderColor.b = 0;
        elevationSliderColor.r = 0.5f - averageElevationGreenness;
        elevationSliderColor.g = 0.5f + averageElevationGreenness;
        elevationSliderColor.a = 1;
        elevationSliderImage.color = elevationSliderColor;
        velocitySlider.value = Mathf.Atan(selfRB.velocity.magnitude / 120) * 2 / Mathf.PI;
        velocityGreenness = Mathf.Atan((selfRB.velocity.magnitude - lastVelocity) / 2) * 2 / Mathf.PI;
        velocitySliderColor.b = 0;
        velocitySliderColor.r = 0.5f - velocityGreenness;
        velocitySliderColor.g = 0.5f + velocityGreenness;
        velocitySliderColor.a = 1;
        velocitySliderImage.color = velocitySliderColor;
        distanceFromCenterSquared = new Vector2(this.transform.position.x, this.transform.position.z).sqrMagnitude;
        distanceFromLavaFallsSquared = (this.transform.position - lavaFallsBase.transform.position).sqrMagnitude;
        previousElevation = elevation;

        this.setPanels();
        if (usingSkyLevel)
        {
            energyRegenerationRate = originalEnergyRegenRate * Mathf.Pow((1 - (elevation / karmanLine)), 2);
            wind1.pitch = 0.7f + 4 * Mathf.Sqrt(1 - elevationSlider.value) * (Mathf.Atan(proximityPoints / 60) / Mathf.PI + 0.025f);
            wind2.pitch = 0.7f + 4 * Mathf.Sqrt(1 - elevationSlider.value) * (Mathf.Atan(proximityPoints / 60) / Mathf.PI + 0.025f);
        }
        else
        {
            wind1.pitch = 0.7f + 4 * Mathf.Max((1 - elevationSlider.value), 0.5f) * (Mathf.Atan(proximityPoints / 60) / Mathf.PI + 0.025f);
            wind2.pitch = 0.7f + 4 * Mathf.Max((1 - elevationSlider.value), 0.5f) * (Mathf.Atan(proximityPoints / 60) / Mathf.PI + 0.025f);
        }

        if (!usingSkyLevel && !usingCaveLevel)
        {
            energySlider.value += (energyRegenerationRate * (1 - wingFlapSpeed) - energyDrainRate * wingFlapSpeed * 2) * Time.deltaTime;
        }
        else
        {
            energySlider.value += (energyRegenerationRate * (1 - wingFlapSpeed) * (2 - (distanceFromCenterSquared / Mathf.Pow(14000, 2))) - energyDrainRate * wingFlapSpeed) * Time.deltaTime;
            energySlider.value += (energyRegenerationRate * (1 - wingFlapSpeed) * (40000 / distanceFromLavaFallsSquared) - energyDrainRate * wingFlapSpeed) * Time.deltaTime;
        }

        //add points for proximity to objects
        proximityPoints = 0;
        rollPoints = 0.025f * Mathf.Abs(Vector3.SignedAngle(this.transform.up, previousUpVector, this.transform.forward));
        previousUpVector = this.transform.up;
        for (int i = 0; i < 18; i++)
        {
            Physics.Raycast(this.transform.position, raycastDirections[i], out raycasts[i], 250 * scaleFactor, proximityCheckerMask);
        }
        foreach (RaycastHit hit in raycasts)
        {
            if (hit.distance != 0)
            {
                proximityPoints += multiplierValue * (1 + rollPoints) * Mathf.Max(1, 250 * scaleFactor / hit.distance);
            }
        }
        previousUpVector = this.transform.up;
        proximityPoints += multiplierValue * 250 * scaleFactor / elevation;
        points += proximityPoints * selfRB.velocity.magnitude / 25000;
        points += rollPoints * 0.5f;
        rollPoints = 0;

        //update camera FOV
        if (lastVelocity == null)
        {
            lastVelocity = selfRB.velocity.magnitude;
        }
        //float goalFOVChange = maxFOVDeviation * ((selfRB.velocity.magnitude - lastVelocity) / accelerationAtMaxZoom);
        playerCamera.fieldOfView = playerCamera.fieldOfView + ((maxFOVDeviation * ((selfRB.velocity.magnitude - lastVelocity) / accelerationAtMaxZoom)) + originalFieldOfView - playerCamera.fieldOfView) / FOVDamper;
        lastVelocity = selfRB.velocity.magnitude;
        previousVelocityVector = selfRB.velocity;

        if (usingSkyLevel || usingCaveLevel)
        {
            //update time and ending beeps
            timeLeft -= Time.deltaTime;
            timeRemaining.text = Mathf.FloorToInt(timeLeft).ToString();
            if (timeLeft < 11 && timeLeft > 0)
            {
                if (Mathf.FloorToInt(timeLeft) < previousSecond)
                {
                    beep.Play();
                }
            }
            previousSecond = Mathf.FloorToInt(timeLeft);
            if (timeLeft <= 0)
            {
                this.endGame();
            }
        }
        else
        {
            timeRemaining.text = Mathf.FloorToInt(Time.time - raceStartTime).ToString();
        }
    }

    public void spawnFlock(int flockID)
    {
        flocks[flockID] = Instantiate(flockSpawner);
        if (usingCaveLevel)
        {
            if (this.transform.position.magnitude < 10000)
            {
                Vector3 spawnSpot = Vector3.zero;
                Vector3 spawnDisplacement = this.transform.forward;
                spawnDisplacement = new Vector3(spawnDisplacement.x, 0, spawnDisplacement.z).normalized;
                if (flockID == 0)
                {
                    spawnDisplacement *= 2000;
                    spawnSpot = this.transform.position + spawnDisplacement;
                    spawnSpot = new Vector3(spawnSpot.x, terrains[activeTerrain].SampleHeight(spawnSpot) + 500, spawnSpot.z);
                }
                else if (flockID == 1)
                {
                    Vector3 horizontalOffset = this.transform.right;
                    horizontalOffset = new Vector3(horizontalOffset.x, 0, horizontalOffset.z).normalized;
                    horizontalOffset *= 1400;
                    spawnDisplacement *= 1400;
                    spawnSpot = this.transform.position + spawnDisplacement + horizontalOffset;
                    spawnSpot = new Vector3(spawnSpot.x, terrains[activeTerrain].SampleHeight(spawnSpot) + 500, spawnSpot.z);
                }
                else if (flockID == 2)
                {
                    Vector3 horizontalOffset = this.transform.right;
                    horizontalOffset = new Vector3(horizontalOffset.x, 0, horizontalOffset.z).normalized;
                    horizontalOffset *= -1400;
                    spawnDisplacement *= 1400;
                    spawnSpot = this.transform.position + spawnDisplacement + horizontalOffset;
                    spawnSpot = new Vector3(spawnSpot.x, terrains[activeTerrain].SampleHeight(spawnSpot) + 500, spawnSpot.z);
                }
                for (int i = 0; i < 5; i++)
                {
                    if (Physics.CheckSphere(spawnSpot, 100))
                    {
                        spawnSpot = new Vector3(spawnSpot.x + 50, spawnSpot.y, spawnSpot.z);
                    }
                    else
                    {
                        break;
                    }
                }
                flocks[flockID].transform.position = spawnSpot;
            }
        }
        else if (usingSkyLevel)
        {
            Vector3 directionToSky = this.transform.position - planetOrbSpawner.transform.position;
            Vector3 spawnSpot = Vector3.zero;
            Vector3 spawnDisplacement = Vector3.Cross(directionToSky, Vector3.Cross(this.transform.forward, directionToSky)).normalized;
            if (flockID == 0)
            {
                spawnDisplacement *= 2500;
                spawnSpot = this.transform.position + spawnDisplacement;
                RaycastHit hit;
                if (Physics.Raycast((spawnSpot - planetOrbSpawner.transform.position) * 1.5f + planetOrbSpawner.transform.position, planetOrbSpawner.transform.position - spawnSpot, out hit, 10000))
                {
                    spawnSpot = planetOrbSpawner.transform.position + (hit.point - planetOrbSpawner.transform.position) * 1.005f;
                }
            }
            else if (flockID == 1)
            {
                Vector3 horizontalOffset = this.transform.right;
                horizontalOffset = Vector3.Cross(this.transform.forward, directionToSky).normalized;
                horizontalOffset *= 1750;
                spawnDisplacement *= 1750;
                spawnSpot = this.transform.position + spawnDisplacement + horizontalOffset;
                RaycastHit hit;
                if (Physics.Raycast((spawnSpot - planetOrbSpawner.transform.position) * 1.5f + planetOrbSpawner.transform.position, planetOrbSpawner.transform.position - spawnSpot, out hit, 10000))
                {
                    spawnSpot = planetOrbSpawner.transform.position + (hit.point - planetOrbSpawner.transform.position) * 1.005f;
                }
            }
            else if (flockID == 2)
            {
                Vector3 horizontalOffset = this.transform.right;
                horizontalOffset = new Vector3(horizontalOffset.x, 0, horizontalOffset.z).normalized;
                horizontalOffset *= -1750;
                spawnDisplacement *= 1750;
                spawnSpot = this.transform.position + spawnDisplacement + horizontalOffset;
                RaycastHit hit;
                if (Physics.Raycast((spawnSpot - planetOrbSpawner.transform.position) * 1.5f + planetOrbSpawner.transform.position, planetOrbSpawner.transform.position - spawnSpot, out hit, 10000))
                {
                    spawnSpot = planetOrbSpawner.transform.position + (hit.point - planetOrbSpawner.transform.position) * 1.005f;
                }
            }
            flocks[flockID].transform.position = spawnSpot;
        }
    }

    public void startRace()
    {
        raceObjects.SetActive(true);
        //manually set array length
        raceCheckpointArrayLength = 49;
        currentCheckpointGoal = 0;
        shouldSpawnCheckpointIndicators = raceHintToggle.isOn;
        energySlider.value = energySlider.maxValue;

        this.transform.position = raceSpawnLocation1.transform.position;
        this.transform.rotation = raceSpawnLocation1.transform.rotation;
        this.giveInitialBoost();
        for (int i = 0; i < raceCheckpointArrayLength; i++)
        {
            raceCheckpoints[i].GetComponent<Renderer>().enabled = false;
            //raceCheckpoints[i].gameObject.SetActive(false);
        }

        raceCheckpoints[0].GetComponent<Renderer>().enabled = true;
        raceCheckpoints[1].GetComponent<Renderer>().enabled = true;
        raceCheckpoints[2].GetComponent<Renderer>().enabled = true;
        //raceCheckpoints[0].gameObject.SetActive(true);
        //raceCheckpoints[1].gameObject.SetActive(true);
        //raceCheckpoints[2].gameObject.SetActive(true);
        raceCheckpoints[0].GetComponent<Renderer>().material = nextColor;
        raceCheckpoints[1].GetComponent<Renderer>().material = secondNextColor;
        raceCheckpoints[2].GetComponent<Renderer>().material = thirdNextColor;

        pointsAtStartOfRace = points;
        raceStartTime = Time.time;
        timeDisplayText.text = "0";
    }

    public void setPanels()
    {
        //set blackout panel opacity
        averageBlackout = 0;
        for (int i = 0; i < blackoutAverager.Length; i++)
        {
            averageBlackout += blackoutAverager[i];
            if (i < blackoutAverager.Length - 1)
            {
                blackoutAverager[i] = blackoutAverager[i + 1];
            }
            else
            {
                if ((previousVelocityVector - selfRB.velocity).magnitude != 120)
                {
                    blackoutAverager[i] = (previousVelocityVector - selfRB.velocity).magnitude;
                }
                else
                {
                    blackoutAverager[i] = 0;
                }
            }
        }
        averageBlackout /= blackoutAverager.Length;
        tempColor.g = 0;
        tempColor.r = 0;
        tempColor.b = 0;
        tempColor.a = averageBlackout / 5;
        blackoutPanel.color = tempColor;

        averageAngleToForward = 0;
        for (int i = 0; i < angleToForwardAverager.Length; i++)
        {
            averageAngleToForward += angleToForwardAverager[i];
            if (i < angleToForwardAverager.Length - 1)
            {
                angleToForwardAverager[i] = angleToForwardAverager[i + 1];
            }
            else
            {
                angleToForwardAverager[i] = forwardAngleToMotion;
            }
        }
        averageAngleToForward /= angleToForwardAverager.Length;

        //set snowflake panel
        if (usingSkyLevel)
        {
            tempColor.g = 1;
            tempColor.r = 1;
            tempColor.b = 1;
            tempColor.a = Mathf.Pow((elevation / karmanLine), 0.5f) - 0.4f;
            snowflakePanel.color = tempColor;
        }
        else
        {
            tempColor.a = 0;
            snowflakePanel.color = tempColor;
        }
    }

    public void finishRace()
    {
        Debug.Log("race finished");
        //raceObjects.SetActive(false);

        //reset player location
        //if (usingSkyLevel)
        //{
        //    this.transform.position = skyStartLocation.transform.position;
        //}
        //else
        //{
        //    this.transform.position = caveStartLocation.transform.position;
        //}
        //this.transform.rotation = Quaternion.identity;
        //this.giveInitialBoost();

        //check for race time record
        if (Time.time - raceStartTime < playerData.raceTimeRecord || playerData.raceTimeRecord == 0)
        {
            newRecord.SetActive(true);
            playerData.raceTimeRecord = Time.time - raceStartTime;
            Debug.Log("race record time " + (Time.time - raceStartTime).ToString("0.00"));
        }
        else
        {
            newRecord.SetActive(false);
        }
        finalScore.gameObject.SetActive(false);
        finalTime.gameObject.SetActive(true);
        finalTime.text = "Time: " + (Time.time - raceStartTime).ToString("0.0");
        this.savePlayerData();
        this.endGame();
    }

    public void raceHintToggleChange()
    {
        playerData.raceIndicator = raceHintToggle.isOn;
        shouldSpawnCheckpointIndicators = raceHintToggle.isOn;
        this.savePlayerData();
    }

    public void mutedToggleChange()
    {
        playerData.muted = mutedToggle.isOn;
        //mainAudioListener.enabled = !playerData.muted;
        if (playerData.muted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
        this.savePlayerData();
    }

    public void musicToggleChange()
    {
        playerData.musicEnabled = musicToggle.isOn;
        this.savePlayerData();
        if (musicToggle.isOn)
        {
            this.playNextTrack();
        }
        else
        {
            songs[0].Stop();
            songs[1].Stop();
            songs[2].Stop();
            currentSong = 100;
        }
    }

    public void musicVolumeChanged(float newVolume)
    {
        foreach (AudioSource song in songs)
        {
            song.volume = newVolume;
            if (song.isPlaying)
            {
                song.Pause();
                song.Play();
            }
        }
        playerData.musicVolume = newVolume;
        if (newVolume == 0)
        {
            foreach (AudioSource song in songs)
            {
                song.Stop();
            }
        }
        else 
        {
            bool aSongIsPlaying = false;
            foreach (AudioSource song in songs)
            {
                if (song.isPlaying)
                {
                    aSongIsPlaying = true;
                }
            }
            if (!aSongIsPlaying)
            {
                this.playNextTrack();
            }
        }
        this.savePlayerData();
    }

    public void gameVolumeChanged(float newVolume)
    {
        AudioListener.volume = newVolume;
        playerData.gameVolume = newVolume;
        this.savePlayerData();
    }

    public void setLocation()
    {
        this.resetLevel();
        if (locationSelecter.value == 0)
        {
            //load sky
            selfRB.useGravity = false;
            usingSkyLevel = true;
            usingCaveLevel = false;
            this.transform.position = skyStartLocation.transform.position;
            skyLight.SetActive(true);
            RenderSettings.skybox = milkyWay;
            elevationSlider.gameObject.SetActive(true);
            pointsText.gameObject.SetActive(true);
            multiplierText.gameObject.SetActive(true);
            lavaFalls.SetActive(false);
            restartButton.SetActive(false);
        }
        else if (locationSelecter.value == 1)
        {
            //load cave
            selfRB.useGravity = true;
            usingSkyLevel = false;
            usingCaveLevel = true;
            this.transform.position = caveStartLocation.transform.position;
            skyLight.SetActive(false);
            RenderSettings.skybox = caveSkybox;
            elevationSlider.gameObject.SetActive(true);
            pointsText.gameObject.SetActive(true);
            multiplierText.gameObject.SetActive(true);
            lavaFalls.SetActive(true);
            restartButton.SetActive(false);
        }
        else if (locationSelecter.value == 2)
        {
            //load race
            selfRB.useGravity = true;
            usingSkyLevel = false;
            usingCaveLevel = false;
            pointsText.gameObject.SetActive(false);
            multiplierText.gameObject.SetActive(false);
            this.transform.position = raceSpawnLocation1.transform.position;
            skyLight.SetActive(false);
            RenderSettings.skybox = caveSkybox;
            elevationSlider.gameObject.SetActive(false);
            lavaFalls.SetActive(false);
            restartButton.SetActive(true);
            raceStartTime = Time.time;
            timeDisplayText.text = "0";
        }
        playerData.location = locationSelecter.value;
        this.savePlayerData();
    }

    public void raceButton()
    {
        this.transform.position = raceTrigger.transform.position;
    }

    public void giveInitialBoost()
    {
        selfRB.velocity = new Vector3(0, 0, 120);
        selfRB.angularVelocity = Vector3.zero;
    }

    public void pauseGame()
    {
        Cursor.visible = true;
        foreach (ShellCrabController crab in shellCrabArray)
        {
            crab.motionSound.Stop();
        }
        spider.motionSound.Stop();
        stoneMonster.motionSound.Stop();
        foreach (VikingShipController ship in ships)
        {
            ship.fireSound.Stop();
        }
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gameIsPaused = true;
        wind1.Pause();
        wind2.Pause();
        highScoreText.text = playerData.highScore.ToString("0.0");
        highScoreTimeText.text = playerData.highScoreTime.ToString("0.0") + " s";
        skyHighScoreText.text = playerData.skyHighScore.ToString("0.0");
        skyHighScoreTimeText.text = playerData.skyHighScoreTime.ToString("0.0") + " s";
        raceRecordTimeText.text = playerData.raceTimeRecord.ToString("0.00") + " s";
        racePointsPerTimeText.text = "Race Points/Time Record: " + playerData.racePointsPerTimeRecord.ToString("0.0");
    }

    public void resumeGame()
    {
        gameOverPanel.SetActive(false);
        newHighscore.SetActive(false);
        newRecord.SetActive(false);
        newAdjustedHighscore.SetActive(false);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gameIsPaused = false;
        wind1.Play();
        wind2.Play();
        Cursor.visible = false;
    }

    public void resetLevel()
    {
        if (usingSkyLevel || usingCaveLevel)
        {
            if (pausedIndicator.gameObject.activeSelf)
            {
                pausedIndicator.gameObject.SetActive(false);
                this.endGame();
            }
        }
        flamethrowerExhausted = false;
        shouldAddPoints = false;
        shouldAddMultiplier = false;
        canUseFlamethrower = true;
        wasUsingFlamethrower = false;
        this.resetPanelAveragers();
        if (usingSkyLevel)
        {
            this.transform.position = skyStartLocation.transform.position;
            pointsText.gameObject.SetActive(true);
            shouldSpawnCheckpointIndicators = false;
            multiplierText.gameObject.SetActive(true);
        }
        else if (usingCaveLevel)
        {
            energyRegenerationRate = originalEnergyRegenRate;
            this.transform.position = caveStartLocation.transform.position;
            pointsText.gameObject.SetActive(true);
            shouldSpawnCheckpointIndicators = false;
            multiplierText.gameObject.SetActive(true);
        }
        else
        {
            energyRegenerationRate = originalEnergyRegenRate;
            shouldSpawnCheckpointIndicators = playerData.raceIndicator;
            this.transform.position = raceSpawnLocation1.transform.position;
            pointsText.gameObject.SetActive(false);
            multiplierText.gameObject.SetActive(false);
            this.startRace();
        }
        for (int i = 0; i < raceShields.Length; i++)
        {
            raceShields[i].SetActive(true);
        }
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].gameObject.SetActive(true);
            ships[i].health = ships[i].originalHealth;
            ships[i].flame1.SetActive(false);
            ships[i].flame2.SetActive(false);
            ships[i].flame3.SetActive(false);
            ships[i].fireSound.Stop();
        }
        this.transform.rotation = Quaternion.identity;
        baseMultiplierValue = 1;
        multiplierText.text = "X" + multiplierValue.ToString("0.0");
        multiplierTime = Time.time;
        points = 0;
        pointsText.text = points.ToString("0.0");
        startTime = Time.time;
        raceStartTime = Time.time;
        selfRB.velocity = new Vector3(0, 0, 120);
        selfRB.angularVelocity = Vector3.zero;
        raceTrigger.SetActive(false);
        previousElevation = elevation;
        previousVelocityVector = selfRB.velocity;
        lastVelocity = selfRB.velocity.magnitude;
        energySlider.value = energySlider.maxValue;
        tucking = false;
        untucking = false;
        untucked = true;
        tucked = false;
        leftWing.transform.localPosition = originalLeftWingLocation;
        leftWing.transform.localRotation = Quaternion.Euler(originalLeftWingRotation);
        rightWing.transform.localPosition = originalRightWingLocation;
        rightWing.transform.localRotation = Quaternion.Euler(originalRightWingRotation);

        this.setPanels();

        //reset time
        gameStartTime = Time.time;
        timeLeft = timeLimit;
        timeRemaining.text = timeLeft.ToString("0");

        this.setGoalToggles();
    }

    public void resetPanelAveragers()
    {
        elevationGreennessAverager = new float[30];
        for (int i = 0; i < elevationGreennessAverager.Length; i++)
        {
            elevationGreennessAverager[i] = 0;
        }
        blackoutAverager = new float[10];
        for (int i = 0; i < blackoutAverager.Length; i++)
        {
            blackoutAverager[i] = 0;
        }
        angleToForwardAverager = new float[20];
        for (int i = 0; i < angleToForwardAverager.Length; i++)
        {
            angleToForwardAverager[i] = 0;
        }
    }

    public void endGame()
    {
        if ((Time.time - gameStartTime) > 0.5f || (Time.time - startTime) > 0.5f)
        {
            wind1.Stop();
            wind2.Stop();
            this.pauseGame();
            gameOverPanel.SetActive(true);
            Debug.Log("Game should have ended");
            if (points > playerData.highScore && usingCaveLevel)
            {
                newHighscore.SetActive(true);
                playerData.highScore = points;
                playerData.highScoreTime = Time.time - gameStartTime;
                Highscores.AddNewHighscore(playerData.highScore, playerData.playerID, playerData.highScoreTime.ToString());
            }
            else if (points > playerData.skyHighScore && usingSkyLevel)
            {
                newHighscore.SetActive(true);
                playerData.skyHighScore = points;
                playerData.skyHighScoreTime = Time.time - gameStartTime;
                Highscores.AddNewHighscore(playerData.skyHighScore, playerData.playerID, playerData.skyHighScoreTime.ToString());
            }
            if (usingSkyLevel || usingCaveLevel)
            {
                finalScore.gameObject.SetActive(true);
                finalTime.gameObject.SetActive(true);
                finalScore.text = "Score: " + points.ToString("0.0");
                finalTime.text = "Time: " + (Time.time - gameStartTime).ToString("0.0");
            }
            else
            {
                
            }
            //if (points * points / (Time.time - startTime) > playerData.adjustedHighScore)
            //{
            //    //notify you got an adjusted high score
            //    Debug.Log("adjusted highscore!");
            //    newAdjustedHighscore.SetActive(true);
            //    playerData.adjustedHighScore = points * points / (Time.time - startTime);
            //}
            this.savePlayerData();
            highScoreText.text = playerData.highScore.ToString("0.0");
            highScoreTimeText.text = playerData.highScoreTime.ToString("0.0") + " s";
            skyHighScoreText.text = playerData.skyHighScore.ToString("0.0");
            skyHighScoreTimeText.text = playerData.skyHighScoreTime.ToString("0.0") + " s";
            raceRecordTimeText.text = playerData.raceTimeRecord.ToString("0.00") + " s";
            racePointsPerTimeText.text = "Race Points/Time Record: " + playerData.racePointsPerTimeRecord.ToString("0.0");
            this.resetLevel();
        }
    }

    public void setGoalToggles()
    {
        if (playerData.demonKills >= 10)
        {
            demonToggle.isOn = true;
        }
        if (playerData.spiderKills >= 10)
        {
            spiderToggle.isOn = true;
        }
        if (playerData.crabKills >= 25)
        {
            crabToggle.isOn = true;
        }
        if (playerData.raceTimeRecord <= 120)
        {
            raceGoalToggle.isOn = true;
        }
        borkToggle.isOn = playerData.borkGateHit;
        if (playerData.highScore >= 10000 || playerData.skyHighScore >= 10000)
        {
            tenKToggle.isOn = true;
        }
        if (playerData.highScore >= 100000 || playerData.skyHighScore >= 100000)
        {
            hundredKToggle.isOn = true;
        }
        if (playerData.highScore >= 1000000 || playerData.skyHighScore >= 1000000)
        {
            millionToggle.isOn = true;
        }
        tenMinuteToggle.isOn = playerData.tenMinuteFlight;
        twentyMinuteToggle.isOn = playerData.twentyMinuteFlight;
    }

    public void setTerrain()
    {
        if (this.transform.position.x >= 0)
        {
            if (this.transform.position.z >= 0)
            {
                activeTerrain = 3;
            }
            else
            {
                activeTerrain = 2;
            }
        }
        else
        {
            if (this.transform.position.z >= 0)
            {
                activeTerrain = 1;
            }
            else
            {
                activeTerrain = 0;
            }
        }
    }

    public void playPointsSound(float greenValue, float blueValue)
    {
        float tempPitch = 0;
        tempPitch = Mathf.Max(greenValue, blueValue * 0.75f, 0.5f);
        pointsSoundEffect.Stop();
        pointsSoundEffect.time = 0;
        pointsSoundEffect.pitch = tempPitch;
        pointsSoundEffect.Play();
    }

    public void playMultiplierSound()
    {
        pointsSoundEffect.pitch = 1;
        pointsSoundEffect.Play();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BorkGate")
        {
            borkToggle.isOn = true;
            playerData.borkGateHit = true;
            this.savePlayerData();
        }
        if (other.gameObject.name == "StartRace")
        {
            other.gameObject.SetActive(false);
            this.startRace();
        }
        //set collider for adding points
        if (other.tag == "Point Claw")
        {
            greenValue = other.GetComponentInChildren<MeshRenderer>().material.color.g;
            blueValue = other.GetComponentInChildren<MeshRenderer>().material.color.b;
            shouldAddPoints = true;
            Destroy(other.gameObject);
            orbSpawner.numberOfOrbs--;
            this.playPointsSound(greenValue, blueValue);
        }

        if (other.tag == "Planet Orb")
        {
            greenValue = 1;
            blueValue = 0;
            shouldAddPoints = true;
            Destroy(other.gameObject);
            planetOrbSpawner.spawnNextOrb();
            this.playPointsSound(1, 0);
        }

        if (other.tag == "Multiplier")
        {
            Destroy(other.transform.parent.gameObject);
            if (usingSkyLevel)
            {
                baseMultiplierValue += 0.5f;
                planetOrbSpawner.spawnNextMultiplier();
            }
            else if (usingCaveLevel)
            {
                shouldAddMultiplier = true;
                orbSpawner.numberOfMultipliers--;
            }
            this.playMultiplierSound();
        }

        if (other.tag == "Multiplier" || other.tag == "Point Claw")
        {
            GameObject explosionTemp = Instantiate(explosion);
            explosionTemp.transform.position = other.transform.position;
            Destroy(explosionTemp, 3);
        }

        //check for race checkpoints
        if (other.tag == "Race Checkpoint")
        {
            if (other.gameObject.name == raceCheckpoints[currentCheckpointGoal].gameObject.name)
            {
                //spawn explosion
                //GameObject temp = Instantiate(checkpointExplosionEffect);
                //temp.transform.position = other.transform.position;
                //Destroy(temp, 4);
                this.playPointsSound(2, 1);
                //check if checkpoint is the last one
                if (currentCheckpointGoal == raceCheckpointArrayLength - 1)
                {
                    other.gameObject.GetComponent<Renderer>().enabled = false;
                    this.finishRace();
                    this.resetLevel();
                }
                else
                {
                    if (currentCheckpointGoal == raceCheckpointArrayLength - 2)
                    {
                        raceCheckpoints[currentCheckpointGoal].GetComponent<Renderer>().enabled = false ;
                        raceCheckpoints[currentCheckpointGoal + 1].GetComponent<Renderer>().enabled = true;
                        raceCheckpoints[currentCheckpointGoal + 1].GetComponent<Renderer>().material = nextColor;
                    }
                    else if (currentCheckpointGoal == raceCheckpointArrayLength - 3)
                    {
                        raceCheckpoints[currentCheckpointGoal].GetComponent<Renderer>().enabled = false;
                        raceCheckpoints[currentCheckpointGoal + 1].GetComponent<Renderer>().enabled = true;
                        raceCheckpoints[currentCheckpointGoal + 2].GetComponent<Renderer>().enabled = true;
                        raceCheckpoints[currentCheckpointGoal + 1].GetComponent<Renderer>().material = nextColor;
                        raceCheckpoints[currentCheckpointGoal + 2].GetComponent<Renderer>().material = secondNextColor;
                    }
                    else
                    {
                        raceCheckpoints[currentCheckpointGoal].GetComponent<Renderer>().enabled = false;
                        raceCheckpoints[currentCheckpointGoal + 1].GetComponent<Renderer>().enabled = true;
                        raceCheckpoints[currentCheckpointGoal + 2].GetComponent<Renderer>().enabled = true;
                        raceCheckpoints[currentCheckpointGoal + 3].GetComponent<Renderer>().enabled = true;
                        raceCheckpoints[currentCheckpointGoal + 1].GetComponent<Renderer>().material = nextColor;
                        raceCheckpoints[currentCheckpointGoal + 2].GetComponent<Renderer>().material = secondNextColor;
                        raceCheckpoints[currentCheckpointGoal + 3].GetComponent<Renderer>().material = thirdNextColor;
                    }
                    currentCheckpointGoal++;
                }
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!usingSkyLevel && !usingCaveLevel)
        {
            finalScore.gameObject.SetActive(false);
            finalTime.text = "Race Failed";
        }
        if (!deathSound.isPlaying)
        {
            deathSound.time = 0;
            deathSound.Play();
        }
        if (collision.collider.gameObject.layer == 19)
        {
            Destroy(collision.collider.gameObject);
        }
        this.endGame();
    }

    public void setSliders()
    {
        tailLength.value = playerData.tailLength * 4;
        tailWidth.value = playerData.tailWidth * 4;
        tailTrail.value = playerData.tailTrail * 4;
        wingWidth.value = playerData.wingWidth * 4;
        wingLength.value = playerData.wingLength * 4;
        bodySize.value = playerData.bodySize * 4;
    }

    public void setWingLength(float k)
    {
        playerData.wingLength = k / 4;
        this.savePlayerData();
        this.updatePartSizes();
    }

    public void setWingWidth(float k)
    {
        playerData.wingWidth = k / 4;
        this.savePlayerData();
        this.updatePartSizes();
    }

    public void setTailTrail(float k)
    {
        playerData.tailTrail = k / 4;
        this.savePlayerData();
        this.updatePartSizes();
    }

    public void setTailWidth(float k)
    {
        playerData.tailWidth = k / 4;
        this.savePlayerData();
        this.updatePartSizes();
    }

    public void setTailLength(float k)
    {
        playerData.tailLength = k / 4;
        this.savePlayerData();
        this.updatePartSizes();
    }

    public void setBodySize(float k)
    {
        playerData.bodySize = k / 4;
        this.savePlayerData();
        this.updatePartSizes();
    }

    public void setOriginalPartSizes()
    {
        originalBodySize = body.transform.localScale;
        originalHeadSize = head.transform.localScale;
        originalWingSize = rightWing.transform.localScale;
        originalTailTrailSize = tailTrailObject.transform.localScale;
        originalTailSize = tail.transform.localScale;
    }

    public void setResolution(int resolutionIndex)
    {
        if (resolutionIndex < resolutions.Length)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            playerData.resolution = resolutionIndex;
            this.savePlayerData();
        }
    }

    public void exit()
    {
        Debug.Log("quit game");
        Application.Quit();
    }

    public void playNextTrack()
    {
        if (currentSong >= numberOfSongs - 1)
        {
            currentSong = UnityEngine.Random.Range(0, numberOfSongs);
            Debug.Log("random song");
        }
        else
        {
            currentSong++;
            songs[currentSong].Play();
            Debug.Log("next track");
        }
        if (!songs[currentSong].isPlaying)
        {
            if (currentSong == numberOfSongs - 1)
            {
                currentSong = 0;
            }
            else
            {
                currentSong++;
            }
            songs[currentSong].time = 0;
            songs[currentSong].Play();
        }
    }

    public void updatePartSizes()
    {
        body.transform.localScale = originalBodySize * playerData.bodySize;
        Vector3 tempWingScale = originalWingSize;
        Vector3 tempTailTrailScale = originalTailTrailSize;
        Vector3 tempTailScale = originalTailSize;
        tempWingScale.x *= playerData.wingLength;
        tempWingScale.z *= playerData.wingWidth;
        leftWing.transform.localScale = tempWingScale;
        rightWing.transform.localScale = tempWingScale;
        tempTailTrailScale.z *= playerData.tailTrail;
        tailTrailObject.transform.localScale = tempTailTrailScale;
        tempTailScale.x *= playerData.tailLength;
        tempTailScale.z *= playerData.tailWidth;
    }

    public void saveGameData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(System.IO.Path.Combine(Application.streamingAssetsPath, "gameData.dat"));
        if (gameData == null)
        {
            gameData = new GameData();
        }
        bf.Serialize(file, gameData);
        file.Close();
        Debug.Log("Game data saved successfully");
    }

    public void loadGameData()
    {
        if (File.Exists(System.IO.Path.Combine(Application.streamingAssetsPath, "gameData.dat")))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(System.IO.Path.Combine(Application.streamingAssetsPath, "gameData.dat"));
            gameData = (GameData)bf.Deserialize(file);
            file.Close();
            Debug.Log("Game data streamed successfully");
        }
        else
        {
            this.saveGameData();
        }
    }

    public void savePlayerData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "playerData.dat");
        if (playerData == null)
        {
            playerData = new PlayerData();
        }
        bf.Serialize(file, playerData);
        file.Close();
        Debug.Log("Player data saved successfully");
    }

    public void loadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "playerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(Application.persistentDataPath + "playerData.dat");
            playerData = (PlayerData)bf.Deserialize(file);
            file.Close();
            Debug.Log("Player data loaded successfully");
        }
        else
        {
            this.savePlayerData();
        }
    }
}

[Serializable]
public class GameData
{

}

[Serializable]
public class PlayerData
{
    public float highScore;
    public float highScoreTime;
    public float skyHighScore;
    public float skyHighScoreTime;
    public float adjustedHighScore;
    public float wingLength = 1;
    public float wingWidth = 1;
    public float tailTrail = 1;
    public float tailWidth = 1;
    public float tailLength = 1;
    public float bodySize = 1;
    public float raceTimeRecord = 999999999;
    public float racePointsPerTimeRecord = 0;
    public float gameVolume = 1f;
    public float musicVolume = 0.05f;
    public int resolution = 123456789;
    public int location = 0;
    public string dragonColor = "yellow";
    public string playerID = "0123456789";
    public bool raceIndicator = true;
    public bool musicEnabled = true;
    public bool muted = false;

    public int demonKills = 0;
    public int spiderKills = 0;
    public int crabKills = 0;
    public bool borkGateHit = false;
    public bool tenMinuteFlight = false;
    public bool twentyMinuteFlight = false;
}

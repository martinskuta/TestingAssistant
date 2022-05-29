package com.jetbrains.rider.plugins.testingassistant

import com.jetbrains.rider.actions.base.RiderAnAction
import icons.ReSharperIcons.UnitTesting

class GotoTestAction : RiderAnAction(
        "TestingAssistant.GotoTest",
        "Go to test",
        null,
        UnitTesting.TestFixtureToolWindow)
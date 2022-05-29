package com.jetbrains.rider.plugins.testingassistant

import com.jetbrains.rider.settings.simple.SimpleOptionsPage

class CognitiveComplexityOptionsPage : SimpleOptionsPage("Testing assistant", "TestingAssistantOptionsPage") {
    override fun getId(): String {
        return "TestingAssistantOptionsPage"
    }
}
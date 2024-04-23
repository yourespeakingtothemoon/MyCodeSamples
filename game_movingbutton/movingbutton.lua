
mb_Window = g_ui.displayUI('movingbutton.otui', modules.game_interface.getRightPanel())
mb_MovingButton = mb_Window:recursiveGetChildById('mb_MovingButton')
function newPostion()
    --move position goes by screen space we need to get the indow position
    local position = mb_Window:getPosition()
    --base the button position off this window position
    --x needs to start on the right side of the window
    local x = (position.x + mb_Window:getWidth()) - mb_MovingButton:getWidth()
    --y needs to be anywhere in the window's height
    local y = position.y + math.random(mb_Window:getHeight())
    mb_MovingButton:move(x, y)
end

function updatePosition()
    --this function is called every 10ms
    --like new position we need to get the window position
    --this could be a script wide variable
    local windowPosition = mb_Window:getPosition()
  -- get the position of the button
    local position = mb_MovingButton:getPosition()
    --move the button to the left bu subtracting the x by one and moving
    mb_MovingButton:move(position.x-1, position.y)
    if(position.x <=  windowPosition.x + mb_MovingButton:getWidth()) then
        newPostion()
    end
end

function init()
    --this could be moved to the online function
    -- but for the sake of simplicity we will keep it here
    g_dispatcher.cycleEvent(updatePosition,10)
end

--this script is missing termination and online and offline functions
--this is for the sake of simplicity and ease of understanding
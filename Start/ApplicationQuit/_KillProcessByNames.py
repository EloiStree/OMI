import psutil
import pygetwindow as gw

def find_process_by_window_name(window_name):
    for window in gw.getWindowsWithTitle(window_name):
        return window.pid

def kill_process_by_window_name(window_name):
    process_pid = find_process_by_window_name(window_name)
    if process_pid is not None:
        try:
            process = psutil.Process(process_pid)
            process.terminate()
            print(f"Process with window name '{window_name}' (PID {process_pid}) terminated.")
        except psutil.NoSuchProcess:
            print(f"Process with window name '{window_name}' (PID {process_pid}) not found.")
    else:
        print(f"No process found with window name '{window_name}'.")

if __name__ == "__main__":
    window_name_to_kill = "UDP 2 vJoys Python" # Replace with the window title you want to target
    kill_process_by_window_name(window_name_to_kill)

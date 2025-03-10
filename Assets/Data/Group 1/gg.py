import pandas as pd
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
import numpy as np

# Load the CSV file into a pandas DataFrame
df = pd.read_csv('d:/Acads/mtp/MTP-E/Assets/Data/Group 1/S24014_2.csv')

# Print the column names to verify they are correct
print(df.columns)

# Convert 'Time' column to datetime objects
df['Time'] = pd.to_datetime(df['Time'], unit='ms')

# Filter out rows where EventType is 'RealtimeData'
realtime_df = df[df['EventType'] == 'RealtimeData'].copy()

# Extract X, Y, and Z coordinates from 'EventPosition(X Y Z)'
def extract_coordinates(position_str):
    try:
        x, y, z = map(float, position_str.split())
        return pd.Series({'X': x, 'Y': y, 'Z': z})
    except:
        return pd.Series({'X': np.nan, 'Y': np.nan, 'Z': np.nan})

realtime_df[['X', 'Y', 'Z']] = realtime_df['EventPosition(X Y Z)'].apply(extract_coordinates)

# Convert X, Y, Z to numeric, handling errors
realtime_df['X'] = pd.to_numeric(realtime_df['X'], errors='coerce')
realtime_df['Y'] = pd.to_numeric(realtime_df['Y'], errors='coerce')
realtime_df['Z'] = pd.to_numeric(realtime_df['Z'], errors='coerce')

# Remove rows with NaN values in X, Y, or Z
realtime_df = realtime_df.dropna(subset=['X', 'Y', 'Z'])

# # Basic line plot of 'Time' vs 'Score'
# plt.figure(figsize=(10, 6))  # Adjust figure size for better readability
# plt.plot(df['Time'], df['Score'])
# plt.xlabel('Time')
# plt.ylabel('Score')
# plt.title('Time vs Score')
# plt.grid(True)  # Add grid for better readability
# plt.show()

# Create the 3D plot
fig = plt.figure(figsize=(12, 8))
ax = fig.add_subplot(111, projection='3d')

# Plot the path
ax.plot(realtime_df['X'], realtime_df['Z'], realtime_df['Y'], marker='o', linestyle='-', markersize=2)

# Set labels and title
ax.set_xlabel('X Coordinate (Ground)')
ax.set_ylabel('Z Coordinate (Ground)')
ax.set_zlabel('Y Coordinate (Up)')
ax.set_title('User Movement Path in 3D Space (Realtime Data)')

# Show the plot
plt.show()
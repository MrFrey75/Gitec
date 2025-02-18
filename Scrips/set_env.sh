#!/bin/bash

# Define client credentials
CLIENT_ID="811105c-5db5-4b19-9db8-7e01ac914ac9"
TENANT_ID="7eaaa2e0-ab22-47ef-a037-5e216f661c16"
CLIENT_SECRET="9Gt8Q~vDVttzhKKvbAPqSIgzAYDsaq4bF8cG-a_a"

# Store environment variables (Permanent)
echo "export GRAPH_CLIENT_ID=\"$CLIENT_ID\"" | sudo tee -a /etc/environment > /dev/null
echo "export GRAPH_TENANT_ID=\"$TENANT_ID\"" | sudo tee -a /etc/environment > /dev/null
echo "export GRAPH_CLIENT_SECRET=\"$CLIENT_SECRET\"" | sudo tee -a /etc/environment > /dev/null

# Load new environment variables without reboot
source /etc/environment

echo "Environment variables set successfully."
echo "Restart your terminal or computer to apply changes globally."
echo "You can also run 'source /etc/environment' to apply changes immediately."

# chmod +x set_env.sh
# sudo ./set_env.sh